using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smashgg_to_liquipedia.Liquipedia
{
    public class LpBracket
    {
        private string header;
        private string bracket;

        public LpBracket(string headerText, string bracketText)
        {
            header = headerText;
            bracket = bracketText;
        }

        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                RaisePropertyChanged("Header");
            }
        }

        public string Bracket
        {
            get { return bracket; }
            set
            {
                bracket = value;
                RaisePropertyChanged("Bracket");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
