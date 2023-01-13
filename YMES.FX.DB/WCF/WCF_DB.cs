using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.DB.WCF
{
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    using System.Data;


    [ToolboxItem(false)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "BasicHttpBinding_IService", Namespace = "http://tempuri.org/")]
    public class WCF_DB : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        /// <remarks/>
        public WCF_DB()
        {
            this.Url = "http://210.105.122.128:5098/wcf";
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IService/GetErrorMsg", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string GetErrorMsg()
        {
            object[] results = this.Invoke("GetErrorMsg", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetErrorMsg(System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetErrorMsg", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public string EndGetErrorMsg(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IService/GetTestDataTable", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Data.DataSet GetTestDataTable()
        {
            object[] results = this.Invoke("GetTestDataTable", new object[0]);
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetTestDataTable(System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetTestDataTable", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public System.Data.DataSet EndGetTestDataTable(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IService/ExecuteQuery", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Data.DataSet ExecuteQuery([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string query, [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)][System.Xml.Serialization.XmlArrayItemAttribute("KeyValueOfstringstring", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)] ArrayOfKeyValueOfstringstringKeyValueOfstringstring[] param)
        {
            object[] results = this.Invoke("ExecuteQuery", new object[] {
                    query,
                    param});
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginExecuteQuery(string query, ArrayOfKeyValueOfstringstringKeyValueOfstringstring[] param, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ExecuteQuery", new object[] {
                    query,
                    param}, callback, asyncState);
        }

        /// <remarks/>
        public System.Data.DataSet EndExecuteQuery(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IService/ExecuteNonQueryObject", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ExecuteNonQueryObject([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string query, [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)][System.Xml.Serialization.XmlArrayItemAttribute("KeyValueOfstringanyType", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)] ArrayOfKeyValueOfstringanyTypeKeyValueOfstringanyType[] param, out int ExecuteNonQueryObjectResult, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool ExecuteNonQueryObjectResultSpecified)
        {
            object[] results = this.Invoke("ExecuteNonQueryObject", new object[] {
                    query,
                    param});
            ExecuteNonQueryObjectResult = ((int)(results[0]));
            ExecuteNonQueryObjectResultSpecified = ((bool)(results[1]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginExecuteNonQueryObject(string query, ArrayOfKeyValueOfstringanyTypeKeyValueOfstringanyType[] param, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ExecuteNonQueryObject", new object[] {
                    query,
                    param}, callback, asyncState);
        }

        /// <remarks/>
        public void EndExecuteNonQueryObject(System.IAsyncResult asyncResult, out int ExecuteNonQueryObjectResult, out bool ExecuteNonQueryObjectResultSpecified)
        {
            object[] results = this.EndInvoke(asyncResult);
            ExecuteNonQueryObjectResult = ((int)(results[0]));
            ExecuteNonQueryObjectResultSpecified = ((bool)(results[1]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IService/ExecuteNonQuery", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ExecuteNonQuery([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string query, [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)][System.Xml.Serialization.XmlArrayItemAttribute("KeyValueOfstringstring", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)] ArrayOfKeyValueOfstringstringKeyValueOfstringstring[] param, out int ExecuteNonQueryResult, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool ExecuteNonQueryResultSpecified)
        {
            object[] results = this.Invoke("ExecuteNonQuery", new object[] {
                    query,
                    param});
            ExecuteNonQueryResult = ((int)(results[0]));
            ExecuteNonQueryResultSpecified = ((bool)(results[1]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginExecuteNonQuery(string query, ArrayOfKeyValueOfstringstringKeyValueOfstringstring[] param, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ExecuteNonQuery", new object[] {
                    query,
                    param}, callback, asyncState);
        }

        /// <remarks/>
        public void EndExecuteNonQuery(System.IAsyncResult asyncResult, out int ExecuteNonQueryResult, out bool ExecuteNonQueryResultSpecified)
        {
            object[] results = this.EndInvoke(asyncResult);
            ExecuteNonQueryResult = ((int)(results[0]));
            ExecuteNonQueryResultSpecified = ((bool)(results[1]));
        }
    }

    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
    public partial class ArrayOfKeyValueOfstringstringKeyValueOfstringstring
    {

        private string keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
    public partial class ArrayOfKeyValueOfstringanyTypeKeyValueOfstringanyType
    {

        private string keyField;

        private object valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
