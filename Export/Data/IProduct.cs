using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExportTemplate.Export
{
    /// <summary>
    /// 产出物数据
    /// </summary>
    public interface IProductData
    {
        /// <summary>
        /// 获取单个产出物
        /// </summary>
        /// <param name="objects">联合主键</param>
        /// <returns>DataSet数据集</returns>
        DataSet GetData(params object[] objects);
        /// <summary>
        /// 获取某类产出物的所有产出物数据
        /// </summary>
        /// <returns>产出物数据集列表</returns>
        List<DataSet> GetAllData();
    }

    public class FormProduct : IProductData
    {
        public DataSet GetData(params object[] objects)
        {
            throw new NotImplementedException();
        }

        public List<DataSet> GetAllData()
        {
            throw new NotImplementedException();
        }
    }

    public class FuncListProduct : IProductData
    {
        public DataSet GetData(params object[] objects)
        {
            //throw new NotImplementedException();
            return new DataSet();
        }

        public List<DataSet> GetAllData()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 单个产出物数据获取接口
    /// </summary>
    /// <param name="objects">数量可变参数</param>
    /// <returns></returns>
    public delegate DataSet OneProductDataDelegate(params object[] objects);
    /// <summary>
    /// 一类产出物数据获取接口
    /// </summary>
    /// <returns></returns>
    public delegate List<DataSet> OneClassProductDataDelegate();

    /// <summary>
    /// 产出物数据工厂
    /// </summary>
    public class ProductDataFactory
    {
        private Dictionary<string, OneProductDataDelegate> _oneProductDataDict = new Dictionary<string, OneProductDataDelegate>();
        private Dictionary<string, OneClassProductDataDelegate> _oneClassProductDataDict = new Dictionary<string, OneClassProductDataDelegate>();

        private static ProductDataFactory _instance;
        static ProductDataFactory()
        {
            _instance = new ProductDataFactory();
        }

        public ProductDataFactory()
        {
            Register("Form", new FormProduct());
            Register("FunctionList", new FuncListProduct());
        }

        public static DataSet GetData(string productName, params object[] objects)
        {
            if (!_instance._oneProductDataDict.ContainsKey(productName)) return null;
            return _instance._oneProductDataDict[productName](objects);
        }

        public static List<DataSet> GetAllData(string productName)
        {
            if (!_instance._oneProductDataDict.ContainsKey(productName)) return null;
            return _instance._oneClassProductDataDict[productName]();
        }

        /// <summary>
        /// 注册产出物数据接口
        /// </summary>
        /// <param name="productName">产出物名称</param>
        /// <param name="productData">单个产出物数据接口</param>
        public static void Register(string productName, OneProductDataDelegate productDataDelegate)
        {
            if (!_instance._oneProductDataDict.ContainsKey(productName))
            {
                _instance._oneProductDataDict.Add(productName, productDataDelegate);
            }
        }

        /// <summary>
        /// 注册产出物数据接口
        /// </summary>
        /// <param name="productName">产出物名称</param>
        /// <param name="productData">一类产出物数据接口</param>
        public static void Register(string productName, OneClassProductDataDelegate productDataDelegate)
        {
            if (!_instance._oneProductDataDict.ContainsKey(productName))
            {
                _instance._oneClassProductDataDict.Add(productName, productDataDelegate);
            }
        }

        /// <summary>
        /// 注册产出物数据接口
        /// </summary>
        /// <param name="productName">产出物名称</param>
        /// <param name="productData">产出物数据接口</param>
        public static void Register(string productName, IProductData productData)
        {
            Register(productName, productData.GetData);
            Register(productName, productData.GetAllData);
        }

        #region 测试区域

        public void test()
        {
            DataSet dataSet = ProductDataFactory.GetData("Form", "");
        }

        //delegate void TestDelegate(params object[] strs);
        //public void test(params object[] strs)
        //{
        //    Console.WriteLine(string.Join(" ", strs));
        //}

        //public void test2(params object[] strs)
        //{
        //    TestDelegate func = test;
        //    func(strs);
        //}

        //public void test4(object[] strs)
        //{
        //    TestDelegate func = test;
        //    func(strs);
        //}

        //public void test3()
        //{
        //    test2("a");
        //    test2("a", "b", "c");
        //    test4(new string[] { "D", "E", "F" });
        //}

        #endregion
    }
}
