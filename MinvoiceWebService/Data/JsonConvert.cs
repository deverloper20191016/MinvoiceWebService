using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinvoiceWebService.Models;
using Newtonsoft.Json.Linq;

namespace MinvoiceWebService.Data
{
    public class JsonConvert
    {
        //public static JObject GetJOjectMaster(Inv inv)
        //{
        //    JObject jObject = new JObject();


        //    return jObject;
        //}

        //private static JArray GetJArrayDetailsProduct(List<Product> products, bool opt)
        //{
        //    var jArrayProducts = new JArray();
        //    foreach (var product in products)
        //    {
        //        var jObjectProduct = new JObject
        //        {
        //            {"inv_itemCode", product.ProdCode},
        //            {"inv_itemName", product.ProdName},
        //            {"inv_unitCode", product.ProdUnit},
        //            {"inv_unitName", product.ProdUnit},
        //            {"inv_unitPrice", product.ProdPrice},
        //            {"inv_quantity", product.ProdQuantity},
        //            {"inv_TotalAmountWithoutVat", product.Amount},
        //            {"inv_vatPercentage", product.ProdVat},
        //            {"inv_vatAmount", product.ProdVatAmount},
        //            {"inv_discountPercentage", product.DiscountRate},
        //            {"inv_discountAmount", product.DiscountAmount},
        //            {"inv_promotion", false},
        //            {"ma_thue", product.MaThue}
        //        };
        //        jArrayProducts.Add(jObjectProduct);
        //    }


        //    var jObject = new JObject
        //    {
        //        {"data", jArrayProducts},
        //        {"tab_id", "TAB00188"}
        //    };
        //    if (opt)
        //        jObject.Add("tab_table", "inv_InvoiceAuthDetail");

        //    var jArrayDetailsProduct = new JArray { jObject };
        //    return jArrayDetailsProduct;
        //}
    }
}