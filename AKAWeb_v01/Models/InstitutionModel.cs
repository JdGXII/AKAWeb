using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AKAWeb_v01.Models
{
    public class InstitutionModel
    {
        public int id { get; set; }
        [Display(Name = "Institution")]
        public string name { get; set; }
        public string website { get; set; }
        public DepartmentModel department { get; set; }
        public AddressModel state { get; set; }
        public bool bachelors { get; set; }
        public bool associates { get; set; }
        public bool masters { get; set; }
        public bool phd { get; set; }

        public string asc_string { get; set; }
        public string bac_string { get; set; }
        public string mas_string { get; set; }
        public string phd_string { get; set; }

        public InstitutionModel()
        {
            id = 0;
            name = "Empty";
            website = "Empty";
            department = null;
            state = null;
        }

        public InstitutionModel(int id, string name, string website, DepartmentModel department, AddressModel state)
        {
            this.id = id;
            this.name = name;
            this.website = website;
            this.department = department;
            this.state = state;
        }

        public InstitutionModel(int id, string name, string website)
        {
            this.id = id;
            this.name = name;
            this.website = website;
        }

        public InstitutionModel(int id, string name, string website, DepartmentModel department, AddressModel state, bool bachelors, bool associates, bool masters, bool phd)
        {
            this.id = id;
            this.name = name;
            this.website = website;
            this.department = department;
            this.state = state;
            this.bachelors = bachelors;
            this.associates = associates;
            this.masters = masters;
            this.phd = phd;
            if (this.bachelors)
            {
                this.bac_string = "Bachelors";
            }
            if (this.associates)
            {
                this.asc_string = "Associates";
            }
            if (this.phd)
            {
                this.phd_string = "Doctoral";
            }
            if (this.masters)
            {
                this.mas_string = "Masters";
            }
        }
    }
}