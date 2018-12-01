using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvHelper1201
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvHeader = new string[] { };
            var csvRecords = new List<MyRecord>();

            var csvText = GetCsvText();
            using (var textReader = new StringReader(csvText))
            using (var csv = new CsvReader(textReader)) {
                var header = GetHeader(csv);
                var records = GetRecords(csv);
                csvHeader = header;
                csvRecords = records;
            }

            using (var textWriter = new StringWriter())
            using (var csv = new CsvWriter(textWriter)) {
                WriteCsv(csv, csvHeader, csvRecords);
                var text = textWriter.ToString();
            }

            Console.ReadKey();
        }

        static void WriteCsv(CsvWriter csv, string[] header, List<MyRecord> records)
        {
            foreach (var v in header) {
                csv.WriteField(v);

            }

            csv.NextRecord();
            foreach (var r in records) {
                csv.WriteRecord(r);
                csv.NextRecord();
            }
        }

        static string[] GetHeader(CsvReader csv)
        {
            csv.Read();
            csv.ReadHeader();
            string[] header= csv.Context.HeaderRecord;
            return header;
        }

        static List<MyRecord> GetRecords(CsvReader csv)
        {
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.RegisterClassMap<MyRecordMap>();

            var rows = csv.GetRecords<MyRecord>();

            var buf = new List<MyRecord>();
            foreach (var r in rows) {
                buf.Add(r);
            }
            return buf;
        }

        static string GetCsvText()
        {
            return @"
名前,住所,番号
山田,東京,1
鈴木,大阪,2
田中,福岡,3
";
        }
    }

    class MyRecord
    {
        public string Name { get; set; }
        public string Name2
        {
            get { return "_" + this.Name; }
        }
        public string Address { get; set; }
        public int No { get; set; }
    }

    class MyRecordMap : ClassMap<MyRecord>
    {
        public MyRecordMap()
        {
            Map(m => m.Name).Name("名前");
            Map(m => m.Address).Name("住所");
            Map(m => m.No).Name("番号");
        }
    }
}
