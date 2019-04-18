import { patientphonenumber } from './patientphonenumber';

export class patient {
  patientid: number;

  forenames: string;

  surname: string;

  dateofbirth: Date | string;

  gender: number;



  patientphonenumbers: patientphonenumber[];
}
