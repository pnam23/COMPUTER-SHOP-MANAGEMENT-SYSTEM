using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computershop.Model
{
    public class ProductModel: INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Capital { get; set; }
        public decimal SalePrice { get; set; }
        public string Status {  get; set; }
        public int Quantity {  get; set; }   
        public string Brand {  get; set; }   
        public string Avatar { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
