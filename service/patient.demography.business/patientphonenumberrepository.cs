using Dapper;
using patient.demography.helpers;
using System.Collections.Generic;
using System.Linq;

namespace patient.demography.business
{
    public class patientphonenumberrepository
    {
        private dbcontext _application;
        private int _utcoffset;

        public patientphonenumberrepository(dbcontext application, int utcoffset)
        {
            _application = application;
            _utcoffset = utcoffset;
        }

        public bool addpatientphonenumber(patientphonenumber input, out patientphonenumber patientphonenumber)
        {
            input.isdeleted = false;
            string cmd = @"INSERT INTO [dbo].[patientphonenumber]
                           ([patientid]
                            ,[phonenumbertype]
                            ,[phonenumber]
                            ,[isdeleted]) OUTPUT INSERTED.*
                            VALUES
                           (@patientid
                           ,@phonenumbertype
                           ,@phonenumber
                           ,@isdeleted); ";
            patientphonenumber = _application.Connection.QuerySingle<patientphonenumber>(cmd, input, Transaction: _application.Transaction);
            return patientphonenumber != null;
        }

        public bool addpatientphonenumber(IList<patientphonenumber> input)
        {
            string cmd = @"INSERT INTO [dbo].[patientphonenumber]
                           ([patientid]
                            ,[phonenumbertype]
                            ,[phonenumber]
                            ,[isdeleted]) OUTPUT INSERTED.*
                            VALUES
                           (@patientid
                           ,@phonenumbertype
                           ,@phonenumber
                           ,@isdeleted); ";
            _application.Connection.Execute(cmd, input, Transaction: _application.Transaction);
            return true;
        }

        public bool getpatientphonenumber(int patientphonenumberid, out patientphonenumber patientphonenumber)
        {
            string cmd = @"SELECT [patientphonenumberid]
                                ,[patientid]
                                ,[phonenumbertype]
                                ,[phonenumber]
                                ,[isdeleted]
                          FROM [dbo].[patientphonenumber]
                         WHERE [patientphonenumberid] = @patientphonenumberid ";

            patientphonenumber = _application.Connection.QueryFirstOrDefault<patientphonenumber>(cmd, new { patientphonenumberid }, Transaction: _application.Transaction);
            return patientphonenumber != null;
        }

        public List<patientphonenumber> getallpatientphonenumbers()
        {
            string cmd = @"SELECT [patientphonenumberid]
                                ,[patientid]
                                ,[phonenumbertype]
                                ,[phonenumber]
                                ,[isdeleted]
                          FROM [dbo].[patientphonenumber]
                         WHERE [isdeleted] = @isdeleted ";

            return _application.Connection.Query<patientphonenumber>(cmd, new { isdeleted = false }, Transaction: _application.Transaction).ToList();
        }

        public List<patientphonenumber> getallpatientphonenumbers(int patientid)
        {
            string cmd = @"SELECT [patientphonenumberid]
                                ,[patientid]
                                ,[phonenumbertype]
                                ,[phonenumber]
                                ,[isdeleted]
                          FROM [dbo].[patientphonenumber]
                         WHERE [patientid] = @patientid AND [isdeleted] = @isdeleted ";

            return _application.Connection.Query<patientphonenumber>(cmd, new { isdeleted = false, patientid }, Transaction: _application.Transaction).ToList();
        }

        public bool updatepatientphonenumber(patientphonenumber patientphonenumber)
        {
            string cmd = @"UPDATE [dbo].[patientphonenumber]
                               SET [phonenumbertype] = @phonenumbertype
                                  ,[phonenumber] = @phonenumber
                             WHERE [patientphonenumberid] = @patientphonenumberid ";
            return _application.Connection.Execute(cmd, patientphonenumber, Transaction: _application.Transaction).Equals(1);
        }

        public bool deletepatientphonenumber(int patientphonenumberid)
        {
            string cmd = @"UPDATE [dbo].[patientphonenumber]
                               SET [isdeleted] = @isdeleted
                             WHERE [patientphonenumberid] = @patientphonenumberid ";
            return _application.Connection.Execute(cmd, new { isdeleted = true, patientphonenumberid }, Transaction: _application.Transaction).Equals(1);
        }

        public bool validatepatientphonenumber(patientphonenumber patientphonenumber, out List<string> errors)
        {
            errors = new List<string>();

            if (patientphonenumber.phonenumber.IsTrulyEmpty())
            {
                errors.Add("Enter phone number");
            }
            else
            {
                patientphonenumber.phonenumber = patientphonenumber.phonenumber.TrulyTrim();
                if (patientphonenumber.phonenumber.Length < 5)
                {
                    errors.Add("Phone number must be at least 5 characters");
                }
                if (patientphonenumber.phonenumber.Length < 3)
                {
                    errors.Add("Phone number cannot be more than 15 characters");
                }
            }

            if (patientphonenumber.phonenumbertypeenum == phonenumbertype.none)
            {
                errors.Add("Select phone number type");
            }

            return errors.Count > 0;
        }
    }
}