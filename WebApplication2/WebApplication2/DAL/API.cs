using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WebApplication2.Models;
using WebApplication2.DAL;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class API
    {
        sep21t22Entities2 db = new sep21t22Entities2();
        Data da = new Data();
        private string urlAddress = "https://entool.azurewebsites.net/SEP21";
        private string urlConnect;
        private string data;

        public Logins logins = null;
        //read data from html
        private string Url(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return data;
            }
            return "";
        }

        //check and get data when login success
        public Logins Check_Login(string email, string pass)
        {
            //url address
            urlConnect = urlAddress + "/Login?Username={0}&Password={1}";
            urlConnect = string.Format(urlConnect, email, pass);

            data = Url(urlConnect);
            if (data != "")
            {
                var login = new Logins.Login();
                try
                {
                    logins = JsonConvert.DeserializeObject<Logins>(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return logins;
            }
            urlConnect = "";
            return null;
        }

        public string LoginSecret(string email, string pass)
        {
            urlConnect = urlAddress + "/Login?Username={0}&Password={1}";
            urlConnect = string.Format(urlConnect, email, pass);
            data = Url(urlConnect);
            if (data != "")
            {
                dynamic Account = JsonConvert.DeserializeObject(data);
                string code = Account.code;
                if (int.Parse(code) == 0)
                {
                    string secret = Account.data.secret;
                    urlConnect = "";
                    return secret;
                }
            }
            urlConnect = "";
            return "";
        }


        public List<Students.Student> GetMember(string id)
        {
            urlConnect = urlAddress + "/GetMembers?courseID={0}";
            urlConnect = string.Format(urlConnect, id);
            data = Url(urlConnect);
            if (data != "")
            {
                //tao noi luu tru vao model
                var Student = new List<Students.Student>();
                //parse data json
                //get data json type array
                Students items = JsonConvert.DeserializeObject<Students>(data);
                foreach (var item in items.data)
                {
                    Student.Add(item);
                }
                return Student;

            }
            return null;
        }

        public string UPPER(string str)
        {
            var i = str.ToCharArray();
            var upper = "";
            foreach (var item in i)
            {
                if (char.IsUpper(item))
                {
                    upper += item;
                }
            }
            return upper;
        }

        public async Task<List<Lessons.Lesson>> TestLesson(string id)
        {
            urlConnect = urlAddress + "/GetCourses?lecturerID={0}";
            urlConnect = string.Format(urlConnect, id);
            data = Url(urlConnect);
            if (data != "")
            {
                //parse data json
                var coure = new List<Lessons.Lesson>();

                //get data json type array
                Lessons Courses = JsonConvert.DeserializeObject<Lessons>(data);
                foreach (var item in Courses.data)
                {
                    coure.Add(item);

                    if (da.checkLesson(UPPER(item.Name)) > 0)
                    {
                        continue;
                    }
                    else
                    {

                        MonHoc mh = new MonHoc();

                        mh.MaMH = UPPER(item.Name);
                        mh.TenMH = item.Name;
                        await da.InsertLesson(mh);
                    }


                }
                foreach (var item in Courses.data)
                {

                    if (da.checkCourse(item.Id) > 0)
                    {
                        continue;
                    }
                    else
                    {
                        KhoaHoc cr = new KhoaHoc();
                        cr.Ma = id;

                        cr.MaKH = item.Id;
                        cr.TenKH = item.Name;
                        cr.MaMH = UPPER(item.Name);
                        await da.InsertCourse(cr);
                    }


                }



                return coure;

            }
            return null;
        }
        //public async Task<List<Lessons.Lesson>> TestLesson1(string id)
        //{
        //    urlConnect = urlAddress + "/GetCourses?lecturerID={0}";
        //    urlConnect = string.Format(urlConnect, id);
        //    data = Url(urlConnect);
        //    if (data != "")
        //    {
        //        //parse data json
        //        var coure = new List<Lessons.Lesson>();

        //        //get data json type array
        //        Lessons Courses = JsonConvert.DeserializeObject<Lessons>(data);

        //        return coure;
        //    }
        //    return null;
        //}
     

    }
}
