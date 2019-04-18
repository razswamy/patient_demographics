using Dapper;
using patient.demography.helpers;
using System;
using System.Collections.Generic;

namespace patient.demography.business
{
    public class patientrepository
    {
        private dbcontext _application;
        private int _utcoffset;

        public patientrepository(dbcontext application, int utcoffset)
        {
            _application = application;
            _utcoffset = utcoffset;
        }

        public bool addpatient(patient input, out patient patient)
        {
            input.isdeleted = false;
            string cmd = @"INSERT INTO [dbo].[patient]
                           ([forenames]
                           ,[surname], [gender]
                           ,[dateofbirth]
                           ,[isdeleted]) OUTPUT INSERTED.*
                            VALUES
                           (@forenames
                           ,@surname, @gender
                           ,@dateofbirth
                           ,@isdeleted); ";
            patient = _application.Connection.QuerySingle<patient>(cmd, input, Transaction: _application.Transaction);
            return patient != null;
        }

        public bool getpatient(int patientid, out patient patient, bool fetchphonenumbers = false)
        {
            patientphonenumberrepository objpatientphonenumberrepository = null;
            try
            {
                string cmd = @"SELECT [patientid]
                              ,[forenames]
                              ,[surname], [gender]
                              ,[dateofbirth]
                              ,[isdeleted]
                          FROM [dbo].[patient]
                         WHERE [patientid] = @patientid ";

                patient = _application.Connection.QueryFirstOrDefault<patient>(cmd, new { patientid }, Transaction: _application.Transaction);

                if (fetchphonenumbers)
                {
                    objpatientphonenumberrepository = new patientphonenumberrepository(_application, _utcoffset);
                    patient.patientphonenumbers = objpatientphonenumberrepository.getallpatientphonenumbers(patientid);
                }
                return patient != null;
            }
            finally
            {
                objpatientphonenumberrepository = null;
            }
        }

        public IEnumerable<patient> getallpatients(bool fetchphonenumbers = false)
        {
            patientphonenumberrepository objpatientphonenumberrepository = null;
            try
            {
                string cmd = @"SELECT [patientid]
                              ,[forenames]
                              ,[surname], [gender]
                              ,[dateofbirth]
                              ,[isdeleted]
                          FROM [dbo].[patient]
                         WHERE [isdeleted] = @isdeleted ";
                var tempdata = _application.Connection.Query<patient>(cmd, new { isdeleted = false }, Transaction: _application.Transaction);
                if (fetchphonenumbers)
                {
                    objpatientphonenumberrepository = new patientphonenumberrepository(_application, _utcoffset);
                    foreach (var x in tempdata)
                    {
                        x.patientphonenumbers = objpatientphonenumberrepository.getallpatientphonenumbers(x.patientid);
                    }
                }
                return tempdata;
            }
            finally
            {
                objpatientphonenumberrepository = null;
            }
        }

        public griddata getpatients(griddata griddata)
        {
            patientphonenumberrepository objpatientphonenumberrepository = null;
            try
            {
                griddata tempdata = new griddata();

                int rowcount = 0;
                griddata = griddata ?? new griddata();
                int.TryParse((griddata.rows ?? "").ToString(), out rowcount);
                rowcount = rowcount > 0 ? rowcount : 10;
                tempdata = new griddata();
                string wherestatement = " WHERE [isdeleted] = @isdeleted " + (griddata.search ? " AND ( [forenames] LIKE @searchkey OR [surname] LIKE @searchkey OR [forenames] LIKE @searchkey OR [gender] = @searchkey ) " : "");

                string countcmd = @"SELECT count([patientid]) as patientcount FROM [patient] " + wherestatement;

                string cmd = @"SELECT [patientid]
                              ,[forenames]
                              ,[surname], [gender]
                              ,[dateofbirth]
                              ,[isdeleted] FROM [dbo].[patient] " + wherestatement + " Order by [patientid] DESC" + _application.PagingStatement;
                var parms = new
                {
                    searchkey = "%" + griddata.searchkey + "%",
                    isdeleted = false,
                    griddata.limit,
                    griddata.offset
                };
                tempdata.total = _application.Connection.ExecuteScalar<int>(countcmd, parms, Transaction: _application.Transaction);

                var tt = _application.Connection.Query<patient>(cmd, parms, Transaction: _application.Transaction);
                objpatientphonenumberrepository = new patientphonenumberrepository(_application, _utcoffset);
                foreach (var x in tt)
                {
                    x.patientphonenumbers = objpatientphonenumberrepository.getallpatientphonenumbers(x.patientid);
                }
                tempdata.rows = tt;
                return tempdata;
            }
            finally
            {
                objpatientphonenumberrepository = null;
            }
        }

        public bool updatepatient(patient patient)
        {
            string cmd = @"UPDATE [dbo].[patient]
                               SET [forenames] = @forenames
                                  ,[surname] = @surname
                                  ,[gender] = @gender
                                  ,[dateofbirth] = @dateofbirth
                             WHERE [patientid] = @patientid ";
            return _application.Connection.Execute(cmd, patient, Transaction: _application.Transaction).Equals(1);
        }

        public bool deletepatient(int patientid)
        {
            string cmd = @"UPDATE [dbo].[patient]
                               SET [isdeleted] = @isdeleted
                             WHERE [patientid] = @patientid ";
            return _application.Connection.Execute(cmd, new { isdeleted = true, patientid }, Transaction: _application.Transaction).Equals(1);
        }

        public bool validatepatient(patient patient, out List<string> errors)
        {
            patientphonenumberrepository objpatientphonenumberrepository = null;

            try
            {
                errors = new List<string>();

                if (patient.forenames.IsTrulyEmpty())
                {
                    errors.Add("Enter forename");
                }
                else
                {
                    patient.forenames = patient.forenames.TrulyTrim();
                    if (patient.forenames.Length < 3)
                    {
                        errors.Add("Forename must be at least 3 characters");
                    }
                    if (patient.forenames.Length < 3)
                    {
                        errors.Add("Forename cannot be more than 50 characters");
                    }
                }

                if (patient.surname.IsTrulyEmpty())
                {
                    errors.Add("Enter surname");
                }
                else
                {
                    patient.surname = patient.surname.TrulyTrim();
                    if (patient.surname.Length < 3)
                    {
                        errors.Add("Surname must be at least 3 characters");
                    }
                    if (patient.surname.Length < 3)
                    {
                        errors.Add("Surname cannot be more than 50 characters");
                    }
                }

                if (patient.gendertype == gendertype.none)
                {
                    errors.Add("Select gender");
                }

                if (patient.dateofbirth == DateTime.MinValue)
                {
                    errors.Add("Enter date of birth");
                }

                for (var x = 0; x < patient.patientphonenumbers.Count; x++)
                {
                    objpatientphonenumberrepository = new patientphonenumberrepository(_application, _utcoffset);
                    if (objpatientphonenumberrepository.validatepatientphonenumber(patient.patientphonenumbers[x], out List<string> temperror))
                    {
                        foreach (var x1 in temperror)
                        {
                            errors.Add($"Phone number #{x + 1}: {x1}");
                        }
                    }
                }



                return errors.Count > 0;
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                objpatientphonenumberrepository = null;
            }
        }
    }
}