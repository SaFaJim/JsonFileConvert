using System;
 
namespace Model
{
    public class Student
    {
        /// <summary>
        /// 
        /// </summary>
        public System.Int64 ID { get; set; }

        public string UUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Type { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public System.String UserCode { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public System.String UserName { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public System.String RealName { get; set; }
          
        /// <summary>
        /// 
        /// </summary>
        public System.String PassWord { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Sex { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public System.String Mobile { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public System.String QQ { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public System.String Email { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public System.Boolean HavePicture { get; set; }
        public int PictureID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.String PictureURL { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 DepartmentID { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 SpecialtyID { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 BaseClassID { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean Approved { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime UpdateDate { get; set; }
         
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Status { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday
        {
            get;
            set;
        }




        /// <summary>
        /// 
        /// </summary>
        public System.String College
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Department
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Class
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Specialty
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Address
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String NamePinYin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String IdentityCardType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String IdentityCardNumber
        {
            get;
            set;
        }

        public string ExamNumber{
            get;set;
        }
    }
}
