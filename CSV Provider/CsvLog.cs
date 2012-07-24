using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace org.btg.Star.Rhapsody.Providers.Csv
{
    public class CsvLog
    {
        private TextFieldParser _parser;
        private List<Frame> frames = new List<Frame>();

        public CsvLog()
        {
            throw new ArgumentException("A log location is required");
        }

        public CsvLog(string log)
        {
            this._parser = new TextFieldParser(log);
            this._parser.TextFieldType = FieldType.Delimited;
            this._parser.SetDelimiters(new string[] {","});

            this._GenerateStructure();
        }

        private void _GenerateStructure()
        {
            while (this._parser.EndOfData)
            {
                string[] row = this._parser.ReadFields();

                SkeletonFrame frame = new SkeletonFrame();

                frame.Id = Convert.ToInt32(row[0]);

                // Grab the skeleton data

            }
        }
    }
}