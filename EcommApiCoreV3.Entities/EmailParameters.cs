using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class EmailParameters
    {
        public EmailParameters()
        {

        }

        public string UserName
        {
            get;
            set;
        }

        public string InterviewDate
        {
            get;
            set;
        }



        public string InterviewTime
        {
            get;
            set;
        }


        public string ApplicationNumber
        {
            get;
            set;
        }


        public string FirstName
        {
            get;
            set;
        }



        public string Email
        {
            get;
            set;
        }


        public string LastName
        {
            get;
            set;
        }


        public string Address
        {
            get;
            set;
        }


        public string Subject
        {
            get;
            set;
        }



        public string XMLFilePath
        {
            get;
            set;

        }
        public string EmailBody
        {
            get;
            set;
        }
        public string VenueName { get; set; }
        public string RoomNo { get; set; }
        public string ParkingInstructions { get; set; }
        public string InterviewType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public int Sessions { get; set; }
        public int BranchId { get; set; }
        public string VerificationLink { get; set; }
        public string Branch { get; set; }
        public string BranchEmail { get; set; }
        public string OfferLetterLink { get; set; }
        public string WOTCLink { get; set; }
        public string Interviewlink { get; set; }
        public string OnlineInterview { get; set; }
        public string GetXML()
        {
            string xmlString = "<UserInfo>";
            EmailParameters EParam = this;

            if (EParam.VerificationLink != null && EParam.VerificationLink != "" && EParam.VerificationLink != string.Empty)
            {
                xmlString += "<VerificationLink>" + EParam.VerificationLink + "</VerificationLink>";
            }


            if (EParam.Sessions != null && EParam.Sessions != 0)
            {
                xmlString += "<Sessions>" + EParam.Sessions + "</Sessions>";
            }

            if (EParam.Duration != null && EParam.Duration != string.Empty && EParam.Duration != "")
            {
                xmlString += "<Duration>" + EParam.Duration + "</Duration>";
            }

            if (EParam.EndTime != null && EParam.EndTime != string.Empty && EParam.EndTime != "")
            {
                xmlString += "<EndTime>" + EParam.EndTime + "</EndTime>";
            }

            if (EParam.StartTime != null && EParam.StartTime != string.Empty && EParam.StartTime != "")
            {
                xmlString += "<StartTime>" + EParam.StartTime + "</StartTime>";
            }


            if (EParam.InterviewType != null && EParam.InterviewType != string.Empty && EParam.InterviewType != "")
            {
                xmlString += "<InterviewType>" + EParam.InterviewType + "</InterviewType>";
            }

            if (EParam.ParkingInstructions != null && EParam.ParkingInstructions != string.Empty && EParam.ParkingInstructions != "")
            {
                xmlString += "<ParkingInstructions>" + EParam.ParkingInstructions + "</ParkingInstructions>";
            }

            if (EParam.RoomNo != null && EParam.RoomNo != string.Empty && EParam.RoomNo != "")
            {
                xmlString += "<RoomNo>" + EParam.RoomNo + "</RoomNo>";
            }


            if (EParam.VenueName != null && EParam.VenueName != string.Empty && EParam.VenueName != "")
            {
                xmlString += "<VenueName>" + EParam.VenueName + "</VenueName>";
            }



            if (EParam.UserName != null && EParam.UserName != string.Empty && EParam.UserName != "")
            {
                xmlString += "<UserName>" + EParam.UserName + "</UserName>";
            }


            if (EParam.InterviewDate != null && EParam.InterviewDate != string.Empty && EParam.InterviewDate != "")
            {
                xmlString += "<InterviewDate>" + EParam.InterviewDate + "</InterviewDate>";
            }

            if (EParam.InterviewTime != null && EParam.InterviewTime != string.Empty && EParam.InterviewTime != "")
            {
                xmlString += "<InterviewTime>" + EParam.InterviewTime + "</InterviewTime>";
            }


            if (EParam.ApplicationNumber != null && EParam.ApplicationNumber != string.Empty && EParam.ApplicationNumber != "")
            {
                xmlString += "<ApplicationNumber>" + EParam.ApplicationNumber + "</ApplicationNumber>";
            }


            if (EParam.FirstName != null && EParam.FirstName != string.Empty && EParam.FirstName != "")
            {
                xmlString += "<FirstName>" + EParam.FirstName + "</FirstName>";
            }


            if (EParam.LastName != null && EParam.LastName != string.Empty && EParam.LastName != "")
            {
                xmlString += "<LastName>" + EParam.LastName + "</LastName>";
            }


            if (EParam.Address != null && EParam.Address != string.Empty && EParam.Address != "")
            {
                xmlString += "<Address>" + EParam.Address + "</Address>";
            }


            if (EParam.Password != null && EParam.Password != string.Empty && EParam.Password != "")
            {
                xmlString += "<Password>" + EParam.Password + "</Password>";
            }

            if (EParam.Email != null && EParam.Email != string.Empty && EParam.Email != "")
            {
                xmlString += "<Email>" + EParam.Email + "</Email>";
            }
            if (EParam.VenueName != null && EParam.VenueName != string.Empty && EParam.VenueName != "")
            {
                xmlString += "<VenueName>" + EParam.VenueName + "</VenueName>";
            }
            if (EParam.OfferLetterLink != null && EParam.OfferLetterLink != string.Empty && EParam.OfferLetterLink != "")
            {
                xmlString += "<OfferLetterLink>" + EParam.OfferLetterLink + "</OfferLetterLink>";
            }
            string sender = "";
            if (EParam.Branch != null && EParam.Branch != string.Empty && EParam.Branch != "")
            {
                xmlString += "<Branch>" + EParam.Branch + "</Branch>";

            }

            if (EParam.BranchEmail != null && EParam.BranchEmail != string.Empty && EParam.BranchEmail != "")
            {
                xmlString += "<BranchEmail>" + EParam.BranchEmail + "</BranchEmail>";
                sender = EParam.Branch;
            }

            xmlString += "<Sender>" + sender + "</Sender>";

            xmlString += "</UserInfo>";
            return xmlString;

        }

        public string Password { get; set; }

    }
}
