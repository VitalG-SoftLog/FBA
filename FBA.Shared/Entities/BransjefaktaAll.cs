//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FBA.Shared.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BransjefaktaAll
    {
        public int Id { get; set; }
        public int orgnr { get; set; }
        public int dunsnr { get; set; }
        public short kommunenr { get; set; }
        public string kommune { get; set; }
        public byte fylkesnr { get; set; }
        public string fylke { get; set; }
        public byte nace2 { get; set; }
        public string nace2_tekst { get; set; }
        public string nace5 { get; set; }
        public string nace5_tekst { get; set; }
        public string regnskapsеr { get; set; }
        public string omsetning { get; set; }
        public string EBITDA { get; set; }
        public string Antall_ansatte { get; set; }
        public string Oms_pr_ansatt { get; set; }
        public string lonnskost_pr_ansatt { get; set; }
        public string driftsmargin { get; set; }
    }
}