using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBA.Models
{
    public class DbStruct
    {
        public string Name;
        public int MaxLength;
        public bool IsString;
        public bool IsNull;
        public byte IntSize; 
        //3 bigint   -2^63 (-9,223,372,036,854,775,808) to 2^63-1 (9,223,372,036,854,775,807) 8 Bytes
        //2 int -2^31 (-2,147,483,648) to 2^31-1 (2,147,483,647) 4 Bytes
        //1 smallint -2^15 (-32,768) to 2^15-1 (32,767) 2 Bytes
        //0 tinyint 0 to 255
        public override string ToString()
        {
            return Name + " " + MaxLength.ToString() + " " + GetSqlType();
        }
        public string GetSqlType()
        {
            if (!IsString)
            {
                switch (IntSize)
                {
                    case 0:
                        return "tinyint";
                    case 1:
                        return "smallint";
                    case 2:
                        return "int";
                    case 3:
                        return "bigint";
                }
            }

            return "string";
        }
    }

}