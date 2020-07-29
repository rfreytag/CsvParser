using CsvParser;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Tests
{
    public class LineCurrent_Tests
    {
        string path = string.Empty;
        string content = string.Empty;

        [SetUp]
        public void Setup_LineCurrent()
        {
            // load the test data
            string basedirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = basedirectory + @"..\..\..\TestData\CountryCodes.csv";
            content = File.ReadAllText(path);
        }

        [Test]
        public void LineCurrent_StandardBehavior_Test()
        {
            var no_basic_exceptions = true;
            var call_by_reference_exception = false;

            ICsvReaderRow line1 = null;
            ICsvReaderRow line2 = null;
            string ele1 = string.Empty;
            string ele2 = string.Empty;

            try
            {
                var csvreader = new CsvParser.CsvReader(new MemoryStream(Encoding.UTF8.GetBytes(content)),
                                                                         Encoding.UTF8);

                csvreader.MoveNext();
                line1 = csvreader.Current;
                ele1 = line1[1];

                csvreader.MoveNext();
                line2 = csvreader.Current;
                ele2 = line2[1];
            }
            catch( Exception ex1)
            {
                no_basic_exceptions = false;
            }

            Assert.True(no_basic_exceptions);

            Assert.AreEqual("A2 (ISO)", ele1);
            Assert.AreEqual("AF", ele2);

            Assert.True(line2.Count > 0);
            Assert.True(line1.Count == 0);  // because call-by-reference;

            try
            {
                ele1 = line1[1]; // should throw an error
            }
            catch( Exception ex2)
            {
                call_by_reference_exception = true;
            }

            Assert.True(call_by_reference_exception);
        }

        [Test]
        public void LineCurrent_ByValue_Test()
        {
            var no_basic_exceptions = true;
            var call_by_reference_exception = false;

            ICsvReaderRow line1 = null;
            ICsvReaderRow line2 = null;
            string ele1 = string.Empty;
            string ele2 = string.Empty;

            try
            {
                var csvreader = new CsvParser.CsvReader(new MemoryStream(Encoding.UTF8.GetBytes(content)),
                                                                         Encoding.UTF8);

                csvreader.MoveNext();
                line1 = csvreader.CurrentByValue;
                ele1 = line1[1];

                csvreader.MoveNext();
                line2 = csvreader.CurrentByValue;
                ele2 = line2[1];
            }
            catch (Exception ex1)
            {
                no_basic_exceptions = false;
            }

            Assert.True(no_basic_exceptions);

            Assert.AreEqual("A2 (ISO)", ele1);
            Assert.AreEqual("AF", ele2);

            Assert.True(line2.Count > 0);
            Assert.True(line1.Count > 0);  // because call-by-reference;

            try
            {
                ele1 = line1[1]; // should throw an error
            }
            catch (Exception ex2)
            {
                call_by_reference_exception = true;
            }

            Assert.False(call_by_reference_exception);
        }

        [Test]
        public void LineCurrent_TakeLast_Test()
        {
            var no_basic_exceptions = true;
            var call_by_reference_exception = false;

            ICsvReaderRow line1 = null;
            ICsvReaderRow line2 = null;
            ICsvReaderRow line3 = null;
            string ele1 = string.Empty;
            string ele2 = string.Empty;
            string ele_2ndOfLast4 = String.Empty;

            try
            {
                var csvreader = new CsvParser.CsvReader(new MemoryStream(Encoding.UTF8.GetBytes(content)),
                                                                         Encoding.UTF8);

                csvreader.MoveNext();
                line1 = csvreader.CurrentByValue;
                ele1 = line1[1];


                csvreader.MoveNext();
                line2 = csvreader.CurrentByValue;
                ele2 = line2[1];

                csvreader.MoveNext();
                line3 = csvreader.CurrentByValue;
                ele_2ndOfLast4 = line3.TakeLast(4).ToList()[1];


            }
            catch (Exception ex1)
            {
                no_basic_exceptions = false;
            }

            Assert.True(no_basic_exceptions);

            Assert.AreEqual("A2 (ISO)", ele1);
            Assert.AreEqual("AF", ele2);
            Assert.AreEqual("355", ele_2ndOfLast4);

            Assert.True(line1.Count > 0);  // because call-by-reference;
            Assert.True(line2.Count > 0);
            Assert.True(line3.Count > 0);

            try
            {
                ele1 = line1[1]; // should throw an error
            }
            catch (Exception ex2)
            {
                call_by_reference_exception = true;
            }

            Assert.False(call_by_reference_exception);
        }
    }
}