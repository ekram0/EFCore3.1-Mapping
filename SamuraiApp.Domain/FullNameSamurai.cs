using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamuraiApp.Domain
{
    [ComplexType]
    public class FullNameSamurai
    {

        public string SureName { get; private set; }
        public string GivenName { get; private set; }
        public string FullName => $"{GivenName} {SureName}";
        public string FullNameReverse => $"{SureName} {GivenName}";


        private FullNameSamurai(string givenName, string surName)
            => (SureName, GivenName) = (surName, givenName);

        private FullNameSamurai() { } // due To reflection
        
        public static FullNameSamurai Create(string givenName, string surName)
            => new FullNameSamurai(givenName, surName);

        public static FullNameSamurai Empty()
            => new FullNameSamurai("", "");

        public bool IsEmpty()
            => String.IsNullOrEmpty(SureName) & String.IsNullOrEmpty(GivenName);

        public override bool Equals(object obj)
        {
            var name = obj as FullNameSamurai;
            return name != null &&
                   SureName == name.SureName &&
                   GivenName == name.GivenName;
        }

        public override int GetHashCode()
        {
            var hashCode = -25638710;
            hashCode = hashCode * -2533855 + EqualityComparer<string>.Default.GetHashCode(SureName);
            hashCode = hashCode * -2533855 + EqualityComparer<string>.Default.GetHashCode(GivenName);
            return hashCode;
        }

        public override string ToString()
            => $"Given Name: {GivenName}, SurName: {SureName} ";

        public static bool operator ==(FullNameSamurai name1, FullNameSamurai name2)
            => EqualityComparer<FullNameSamurai>.Default.Equals(name1, name2);

        public static bool operator !=(FullNameSamurai name1, FullNameSamurai name2)
            => !(name1 == name2);


    }
}
