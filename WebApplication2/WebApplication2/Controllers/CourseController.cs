using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CourseController : Controller
    {
        public HttpClient client = new HttpClient();
        public API api = new API();
        sep21t22Entities2 db = new sep21t22Entities2();
        public static string makhdiemdanh = "";
        // GET: Student
        public ActionResult Index(string id)
        {
            string ma = Session["MaGV"].ToString();
            Session["MaKH"] = id;
            //  string ID = Session["ID"].ToString();
            var model = db.KhoaHocs.Where(x => x.MaKH == id && x.Ma == ma);
            // var bh = db.BuoiHocs.Where(x => x.MaKH == id);
            //ViewBag.Lesson = bh;
            return View(model);
        }
        // danh sach sinh vien vaf danh sach bang tham du
        public ActionResult ListStudent(string id)
        {


            Session["MaKH"] = id;
            //  ReturnmaKH(id);
            //  int ID = int.Parse(id);
            // var model = db.ThamDus.Where(x => x.MaKH == id);

            var data = api.GetMember(id);
            foreach (var item in data)
            {
                if (db.SinhViens.Where(x => x.MSSV == item.id).Count() == 0)
                {

                    SinhVien nStudent = new SinhVien();
                    nStudent.MSSV = item.id;
                    nStudent.Birthday = Convert.ToDateTime(item.birthday);
                    nStudent.FullName = item.fullname;
                    nStudent.LastName = item.lastname;
                    nStudent.FirstName = item.firstname;
                    db.SinhViens.Add(nStudent);
                    db.SaveChanges();
                }
                if(db.ThamDus.Where(x=>x.MSSV==item.id && x.MaKH == id).Count() == 0)
                {
                    ThamDu nThamDus = new ThamDu();
                    nThamDus.MSSV = item.id;
                    nThamDus.MaKH = id;
                    db.ThamDus.Add(nThamDus);
                    db.SaveChanges();
                }
            }

            ViewBag.ID = id;
            var masva = db.ThamDus.Where(x => x.MaKH == id);
            var mssv = masva.Select(p => p.MSSV);
            List<SinhVien> list = new List<SinhVien>();
          
            foreach(var item in mssv)
            {
                var sv = db.SinhViens.FirstOrDefault(x => x.MSSV == item);

                list.Add(sv);
            }
            
            return View(list);
        }
        // danh sach buoi hc
        public ActionResult ListBuoi(string id)
        {
            var model = db.BuoiHocs.Where(x => x.MaKH == id);
            return View(model);
        }

        public ActionResult ListDiemDanh(string id)

        {
            try
            {
                if (id == null || id == "" || id.Length < 1)
                {
                    var a = db.BuoiHocs.OrderByDescending(y => y.ID_BH).FirstOrDefault().ID_BH;
                    id = a.ToString();
                }
                Session["BH"] = id;
                //string ids = Session["ID_BH"].ToString();
                //string ids = id;
                int ID = int.Parse(id);
                var model = db.DiemDanhs.Where(x => x.ID_BuoiHoc == ID);
                var makh = db.BuoiHocs.FirstOrDefault(x => x.ID_BH == ID).MaKH;
                ViewBag.BuoiHoc = new SelectList(db.BuoiHocs.OrderByDescending(y => y.ID_BH).Where(x => x.MaKH == makh), "ID_BH", "Buoi_thu");

                //var query = (from pro in db.DiemDanhs where pro.ID_BuoiHoc == ID select pro).ToList();
                ViewBag.Date = db.BuoiHocs.Where(x => x.MaKH == makh).ToList();
                return View(model);
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", "Course", new { id = Session["MaKH"].ToString() });
            }




        }
        //change trang thai buoi diem danh
        public ActionResult Change(string id)
        {
            var ID = int.Parse(id);
            var ID_DD = db.DiemDanhs.FirstOrDefault(x => x.ID == ID);
            if (ID_DD.status == true)
            {
                ID_DD.status = false;
            }
            else
            {
                ID_DD.status = true;
            }
            db.SaveChanges();
            return RedirectToAction("ListDiemDanh", new { id = ID_DD.ID_BuoiHoc });
        }
        // edit change buoi diem danh
        [HttpPost]
        public ActionResult edit(string Buoithu)
        {
            return RedirectToAction("ListDiemDanh", new { id = Buoithu });
        }

        /// ----------------------------------------diem danh thu cong
        //tao buoi ngay luc chon diem danh
        public int taobuoi(string id)
        {
            BuoiHoc bhoc = new BuoiHoc();
            bhoc.NgayHoc = Convert.ToDateTime(DateTime.Now);
            bhoc.Buoi_thu = db.BuoiHocs.Where(x => x.MaKH == id).Count() + 1;
            bhoc.MaKH = id;
            db.BuoiHocs.Add(bhoc);
            db.SaveChanges();
            int ii = bhoc.ID_BH;
            return ii;
        }

        // list danh sach MSSV theo khoa hc
        [HttpGet]
        public ActionResult DIEMDANH(string id)
        {

            Session["MaKH"] = id;
            var list = db.ThamDus.Where(x => x.MaKH == id).ToList();

            return View(list);

        }
        // xu li diem dnah thu cong
        [HttpPost]
        public ActionResult Ed(string bhoc)
        {
            string maGV = Session["MaGV"] as string;
            string maKh = Session["MaKH"] as string;
            // Tao buoi hoc
            int bh = taobuoi(maKh);



            //---------------------------

            var Fch = Request.Form.AllKeys.Where(k => k != "bhoc");
            //string day = DateTime.Now.DayOfWeek.ToString();
            //var query = (from pro in db.BangBuoiHocs where pro.maKH == maKh && pro.day == day select pro.maBH).FirstOrDefault();
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            string time = DateTime.Now.ToString("HH:mm:ss");
            //CreateSessionID create = new CreateSessionID();
            //int getID = (create.GetLastID("DiemDanh", "sessionID", maKh) + 1);

            foreach (var fea in Fch)
            {
                try
                {
                    if (fea.Trim().Length == 7)
                    {
                        DiemDanh dd = new DiemDanh();
                        bool stt = Request.Form[fea.Trim()] != "false";
                        dd.ID_BuoiHoc = bh;
                        dd.MSSV = fea.Trim();
                        dd.status = stt;
                        db.DiemDanhs.Add(dd);
                        db.SaveChanges();
                    }

                }
                catch (Exception)
                {

                }

            }



            //var monhoc = connect.TestCourse(maGV);


            var idbt = db.BuoiHocs.Where(x => x.ID_BH == bh).FirstOrDefault();
            string ids = idbt.Buoi_thu.ToString();
            //ViewBag.Subject = monhoc;
            return RedirectToAction("ListDiemDanh", new RouteValueDictionary(
             new { controller = "Course", action = "ListDiemDanh", id = ids }));

            //return View();
        }


        //Export
        [HttpGet]
        public void ExportToExcel(string idss)
        {

            //int sessionex = int.Parse(Session["SessionExcel"].ToString());
            int idnh = int.Parse(idss);
            List<DiemDanh> diemDanh = db.DiemDanhs.Where(x => x.ID_BuoiHoc == idnh).ToList();
            BuoiHoc time = db.BuoiHocs.Where(x => x.ID_BH == idnh).FirstOrDefault();
            //   List<ThamDu> thamsu = db.ThamDus.Where(x=>x.MaKH == Session["MaKH"].ToString()).ToList();
            List<SinhVien> sinhVien = db.SinhViens.ToList();
            List<DiemDanhview> emplist = new List<DiemDanhview>();
            foreach (var dd in diemDanh)
            {

                foreach (var sv in sinhVien)
                {
                    if (sv.MSSV.Trim() == dd.MSSV.Trim())
                    {
                        DiemDanhview ddModel = new DiemDanhview
                        {
                            MSSV = sv.MSSV,
                            Firstname = sv.FirstName,
                            Lastname = sv.LastName,
                            Birthday = (DateTime)sv.Birthday,
                            DiemDanh = dd.status,
                        };
                        emplist.Add(ddModel);
                    }

                }


            }
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Course";
            ws.Cells["B1"].Value = time.MaKH;

            ws.Cells["A2"].Value = "Date";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", time.NgayHoc);


            ws.Cells["A6"].Value = "ID";
            ws.Cells["B6"].Value = "Last Name";
            ws.Cells["C6"].Value = "First Name";
            ws.Cells["D6"].Value = "Birhday";
            ws.Cells["E6"].Value = "Attendance";

            int rowStart = 7;
            foreach (var item in emplist)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.MSSV;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Lastname;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Firstname;
                ws.Cells[string.Format("D{0}", rowStart)].Value = string.Format("{0:dd/MM/yyyy}", item.Birthday);
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.DiemDanh;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        }


        // sync Members to  API
        public async System.Threading.Tasks.Task<ActionResult> SynMember(string id)
        {
            // SYNC MEMBERS (can` string id cua course)
            // START
            Session["MaKH"] = id;
            var item = db.ThamDus.Where(x => x.MaKH == id).ToList();

            string lecid = (string)Session["MaGV"];
            string sec = (string)Session["secret"];

            ThamDu Member = new ThamDu();
            string[] sID = new string[item.Count()];
            int i = 0;
            foreach (var items in item)
            {
                sID[i] = items.MSSV.ToString();
                i++;
            }

            Members mem = new Members();
            mem.course = id;
            mem.members = sID;

            string json = JsonConvert.SerializeObject(mem);
            var values = new Dictionary<string, string>
                                {
                                    { "uid", lecid },
                                    { "secret", sec },
                                    { "data", json }
                                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://entool.azurewebsites.net/SEP21/SyncMembers", content);
            var responseString = await response.Content.ReadAsStringAsync();
            GetResponeMessage ResponeMessage = JsonConvert.DeserializeObject<GetResponeMessage>(responseString);
            Session["SynMessage"] = ResponeMessage.message.ToString();
            // END

            return RedirectToAction("ListStudent", "Course", new { id = (string)Session["MaKH"] });
        }


        //Sync Attendance to API

        public async System.Threading.Tasks.Task<ActionResult> syncAttendanceAsync()
        {
            HttpClient client = new HttpClient();

            string id = (string)Session["MaKH"];
            string lecid = (string)Session["MaGV"];
            string sec = (string)Session["secret"];

            var value = new
            {
                course = id,
                sessions = db.BuoiHocs.Where(x => x.MaKH == id).AsEnumerable().Select(b => new
                {
                    id = b.Buoi_thu,
                    date = DateTime.Parse(b.NgayHoc.ToString()).ToString("yyyy-MM-ddThh:mm:ss"),
                    info = string.Empty
                }).ToArray(),
                attendance = db.DiemDanhs.AsEnumerable().Where(x => x.BuoiHoc.MaKH == id).Select(m => new
                {
                    student = m.MSSV,
                    checklist = db.DiemDanhs.Where(x => x.status == true && x.BuoiHoc.MaKH == id && x.MSSV== m.MSSV).Select(z => z.BuoiHoc.Buoi_thu).ToArray(),
                    info = string.Empty
                })
            };
            var json = JsonConvert.SerializeObject(value);

            var values = new Dictionary<string, string>
                                {
                                    { "uid", lecid },
                                    { "secret", sec },
                                    { "data", json }
                                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://entool.azurewebsites.net/SEP21/SyncAttendance", content);
            var responseString = await response.Content.ReadAsStringAsync();

            GetResponeMessage ResponeMessage = JsonConvert.DeserializeObject<GetResponeMessage>(responseString);
            //	Session["SynMessage"] = ResponeMessage.message.ToString();

            return RedirectToAction("ListDiemDanh", new { id = Session["BH"] });

        }



    }
}