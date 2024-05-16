using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computershop.Model
{
    public class OrderModel
    {
        public int OrderID {  get; set; }
        public ObservableCollection<ProductModel> ordProductList { get; set; }
        public int CustomerID {  get; set; }
        public string CustomerName {  get; set; }
        public string CustomerPhone {  get; set; }
        public string CustomerEmail {  get; set; }
        public string CustomerAddress {  get; set; }
        public DateTime CreateDate {  get; set; }
        public Decimal Total {  get; set; }
        public Decimal Profit {  get; set; }
        public string Status { get; set; }

    }
}
