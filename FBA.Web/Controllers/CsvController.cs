using FBA.Data;
using FBA.Models;
using FBA.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HOK.Data.Helpers;
using FBA.Data;
using System.Data.SqlClient;
using System.Data;

namespace FBA.Controllers
{
// <configuration>
//  <system.web>
//    <httpRuntime targetFramework="4.5" maxRequestLength="50000" />
//   </system.web>
//  <system.webServer>
//    <security>
//     <requestFiltering>
//        <requestLimits maxAllowedContentLength="500000000" />
//     </requestFiltering>
//    </security>
//  </system.webServer>
//</configuration>
 
    public class CsvController : Controller
    {
        private readonly IFBADataContext ctx;
        public CsvController(IFBADataContext dataContext)
        {
            ctx = dataContext;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Csv Page";

            return View();
        }

        public ActionResult IsertInDB()
        {
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                DoInsert(file.InputStream);
            }

            return View();
        }

        public ActionResult Analyse(bool flag)
        {
            DbStruct[] dbstruct = null;

            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                dbstruct = DoAnalyse(file.InputStream, flag);
            }

            return View(dbstruct);
        }
        private DbStruct[] DoAnalyse(Stream fs, bool flag)
        {
            DbStruct[] dbstruct = null;

            bool isFirstLine = true;
            int TotalColumns = 0, LinesProcessed = 0;

            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitted = line.Split(new char[] { ',' });

                    if (isFirstLine)
                    {
                        TotalColumns = splitted.Length;
                        dbstruct = new DbStruct[TotalColumns];

                        for (int i = 0; i < TotalColumns; i++)
                        {
                            string str = splitted[i];
                            str = str.TrimStart('"').TrimEnd('"');

                            dbstruct[i] = new DbStruct();
                            dbstruct[i].Name = str;
                            dbstruct[i].MaxLength = 0;
                            dbstruct[i].IsString = false;
                            dbstruct[i].IntSize = 0;
                            dbstruct[i].IsNull = false;
                        }

                        isFirstLine = !isFirstLine;
                    }
                    else
                    {
                        for (int i = 0; i < TotalColumns; i++)
                        {
                            string str = splitted[i];
                            str = str.TrimStart('"').TrimEnd('"');

                            
                            if (str == "" || str== string.Empty)
                                dbstruct[i].IsNull = true;

                            if (str.Length > dbstruct[i].MaxLength)
                                dbstruct[i].MaxLength = str.Length;

                            long value = 0;
                            if (!long.TryParse(str, out value))
                                dbstruct[i].IsString = true;
                            else
                                if(flag && value != 0 && str[0] == '0')
                                    dbstruct[i].IsString = true;

                            if (!dbstruct[i].IsString)
                            {
                                byte IntSize = 0;
                                if (value >= 0 && value <= 255)
                                    IntSize = 0;
                                if ((value > 255 && value <= 32767) || (value > -32768 && value < 0))
                                    IntSize = 1;
                                if ((value > 32767 && value <= 2147483647) || (value > -2147483648 && value <= -32768))
                                    IntSize = 2;
                                if ((value > 2147483647 && value <= 9223372036854775807) || (value > -9223372036854775808 && value <= -2147483648))
                                    IntSize = 3;

                                dbstruct[i].IntSize = Math.Max(dbstruct[i].IntSize, IntSize);
                            }
                        }
                    }
                    LinesProcessed++;
                }
            }

            return dbstruct;
        }


        private void DoInsert(Stream fs)
        {
            bool isFirstLine = true;
            int TotalColumns = 0, LinesProcessed = 0;

            int counter = 0;

            System.Data.Entity.DbContext c = (System.Data.Entity.DbContext) ctx;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            c.Configuration.AutoDetectChangesEnabled = false;
            c.Configuration.ValidateOnSaveEnabled = false;

            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitted = line.Split(new char[] { ',' });

                    if (isFirstLine)
                    {
                        TotalColumns = splitted.Length;

                        isFirstLine = !isFirstLine;
                    }
                    else
                    {
                        var obj = new bransjefakta();

                        for (int i = 0; i < TotalColumns; i++)
                        {
                            string str = splitted[i];
                            str = str.TrimStart('"').TrimEnd('"');

                            switch(i)
                            {
                                case 0:
                                case 1:
                                    {
                                        int value = 0;
                                        if (int.TryParse(str, out value))
                                        {
                                            if (i == 0)
                                                obj.orgnr = value;
                                            if (i == 1)
                                                obj.dunsnr = value;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        short value = 0;
                                        if (short.TryParse(str, out value))
                                                obj.kommunenr = value;
                                        break;
                                    }
                                case 4:
                                case 6:
                                    {
                                        byte value = 0;
                                        if (byte.TryParse(str, out value))
                                        {
                                            if (i == 4)
                                                obj.fylkesnr = value;
                                            if (i == 6)
                                                obj.nace2 = value;
                                        }
                                        break;
                                    }
                                case 3:
                                    obj.kommune = str;
                                    break;
                                case 5:
                                    obj.fylke = str;
                                    break;
                                case 7:
                                    obj.nace2_tekst = str;
                                    break;
                                case 8:
                                    obj.nace5 = str;
                                    break;
                                case 9:
                                    obj.nace5_tekst = str;
                                    break;
                                case 10:
                                    obj.regnskapsår = str;
                                    break;
                                case 11:
                                    obj.omsetning = str;
                                    break;
                                case 12:
                                    obj.EBITDA = str;
                                    break;
                                case 13:
                                    obj.Antall_ansatte = str;
                                    break;
                                case 14:
                                    obj.Oms_pr_ansatt = str;
                                    break;
                                case 15:
                                    obj.lonnskost_pr_ansatt = str;
                                    break;
                                case 16:
                                    obj.driftsmargin = str;
                                    break;
                            }
                        }

                        ctx.bransjefakta.Add(obj);
                    }
                    LinesProcessed++;


                    if(LinesProcessed / 10000 > counter)  // 1k - 16sec //10k - 2m50sec
                    {
                        watch.Restart();

                        counter++;
                        ctx.SaveChanges();

                        watch.Stop();
                        var elapsedMs = watch.Elapsed.ToString();
                    }
                }
                ctx.SaveChanges();

            }
        }

    }
}
