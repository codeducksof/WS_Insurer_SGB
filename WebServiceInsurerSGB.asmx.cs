using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Newtonsoft.Json;
using WS_Insurer_SGB.Models;
using System.Security.Cryptography;
using static WS_Insurer_SGB.Models.KPI_GET_QUOTATION;
using System.Net.NetworkInformation;
using System.Xml.Linq;


namespace WS_Insurer_SGB
{
    /// <summary>
    /// Summary description for WebServiceInsurerSGB
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceInsurerSGB : System.Web.Services.WebService
    {

        [WebMethod]
        public string FN_JMI_GET_QUOTATION(string type, string Car_brand, string Car_model, string Car_year, string CC, string Car_type_code)
        {


            var Culture = new CultureInfo("en-US");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            var Result = new M_OUTPUT_JMI();
            var ResultList = new List<M_OUTPUT_JMI>();

            string Response = string.Empty;
            //var Intput = new M_INPUT_MSIG();

            string Status = string.Empty;

            //string strUser = ConfigurationSettings.AppSettings["strAppUserJMI"];
            //string strPassword = ConfigurationSettings.AppSettings["strAppPasswordJMI"];         
            string UrlWs = ConfigurationSettings.AppSettings["strUrlJMI"];

            //Byte[] textEncode = System.Text.Encoding.UTF8.GetBytes(strPassword + ":" + strPassword);
            string Token = ConfigurationSettings.AppSettings["strAppTokenJMI"];

            var body = "";


            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient(UrlWs + "/service/getmapping/");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic " + Token.ToString());

                var Arr_Body = new
                {
                    type = type.ToString(),
                    Car_brand = Car_brand.ToString(),
                    Car_model = Car_model.ToString(),
                    Car_year = Car_year.ToString(),
                    CC = CC.ToString(),
                    Car_type_code = Car_type_code.ToString()
                };

                //var bodyHash = JsonConvert.SerializeObject(Arr_Body);               

                body = JsonConvert.SerializeObject(Arr_Body);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                Status = "SUCCESS";
                Response = response.Content;
                Result = serializer.Deserialize<M_OUTPUT_JMI>(response.Content);

                FN_KEEP_LOG(body, Status, "JMI");
                FN_KEEP_LOG(Response, Status, "JMI");


            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG(body, Status, "JMI");
            }
            finally
            {



            }

            return new JavaScriptSerializer().Serialize(Result);


        }



        [WebMethod]
        public string FN_MSIG_GET_QUOTATION(string action, string make, string model, string packagecode, string packagename, string incdate, string expdate, string name_driv, string inccmi, string veh_cd, string cover_type, string repair_type, string channel, string regis_year, decimal tf_si, decimal od_si, string payment_method, string agentid, string clienttype, string inclcctv, string appid, decimal suminsured, string seat, string cc, decimal tonnage)
        {
            var Culture = new CultureInfo("en-US");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            var Result = new M_OUTPUT_MSIG();
            var ResultList = new List<M_OUTPUT_MSIG>();
            string Response = string.Empty;
            var Intput = new M_INPUT_MSIG();

            string Status = string.Empty;
            string Token = FN_MSIG_GEN_TOKEN();
            string ClientId = ConfigurationSettings.AppSettings["strAppIDMsig"];
            string ClientToken = ConfigurationSettings.AppSettings["strHash"];
            string ClientCookies = ConfigurationSettings.AppSettings["strCookie"];
            string UrlWs = ConfigurationSettings.AppSettings["strUrlMSIG"];
            var body = "";



            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient(UrlWs + "/motor/auth2/MotorCalPremService");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("x-api-key", ClientId.ToString());
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + Token.ToString());
                request.AddHeader("Cookie", ClientCookies.ToString());

                var Arr_Body = new
                {
                    action = action.ToString(),
                    make = make.ToString(),
                    model = model.ToString(),
                    packagecode = packagecode.ToString(),
                    packagename = packagename.ToString(),
                    incdate = incdate.ToString(),
                    expdate = expdate.ToString(),
                    name_driv = name_driv.ToString(),
                    inccmi = inccmi.ToString(),
                    veh_cd = veh_cd.ToString(),
                    cover_type = cover_type.ToString(),
                    repair_type = repair_type.ToString(),
                    channel = channel.ToString(),
                    regis_year = regis_year.ToString(),
                    tf_si = tf_si,
                    od_si = od_si,
                    payment_method = payment_method.ToString(),
                    agentid = agentid.ToString(),
                    clienttype = clienttype.ToString(),
                    inclcctv = inclcctv.ToString(),
                    appid = ClientId.ToString(),
                    suminsured = suminsured,
                    seat = seat.ToString(),
                    cc = cc.ToString(),
                    tonnage = tonnage,

                };

                var bodyHash = JsonConvert.SerializeObject(Arr_Body);
                var hashvalue = FN_MSIG_GEN_HASH(bodyHash.ToString(), Token);

                var Arr_BodyHash = new
                {
                    action = action.ToString(),
                    make = make.ToString(),
                    model = model.ToString(),
                    packagecode = packagecode.ToString(),
                    packagename = packagename.ToString(),
                    incdate = incdate.ToString(),
                    expdate = expdate.ToString(),
                    name_driv = name_driv.ToString(),
                    inccmi = inccmi.ToString(),
                    veh_cd = veh_cd.ToString(),
                    cover_type = cover_type.ToString(),
                    repair_type = repair_type.ToString(),
                    channel = channel.ToString(),
                    regis_year = regis_year.ToString(),
                    tf_si = tf_si,
                    od_si = od_si,
                    payment_method = payment_method.ToString(),
                    agentid = agentid.ToString(),
                    clienttype = clienttype.ToString(),
                    inclcctv = inclcctv.ToString(),
                    appid = ClientId.ToString(),
                    suminsured = suminsured,
                    seat = seat.ToString(),
                    cc = cc.ToString(),
                    tonnage = tonnage,
                    hashvalue = hashvalue.ToString()

                };

                body = JsonConvert.SerializeObject(Arr_BodyHash);

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                Status = "SUCCESS";
                Response = response.Content;
                Result = serializer.Deserialize<M_OUTPUT_MSIG>(response.Content);

                FN_KEEP_LOG(body, Status, "MSIG");
                FN_KEEP_LOG(Response, Status, "MSIG");


            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG(body, Status, "MSIG");
            }
            finally
            {

            }

            return new JavaScriptSerializer().Serialize(Result);
        }

        public string FN_MSIG_GEN_HASH(string Json, string Token)
        {

            var hash = new StringBuilder(); ;
            byte[] secretkeyBytes = Encoding.UTF8.GetBytes(ConfigurationSettings.AppSettings["strHashKey"]);
            byte[] inputBytes = Encoding.UTF8.GetBytes(Json);
            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();

        }

        public string FN_MSIG_GEN_TOKEN()
        {


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            string token = string.Empty;
            string Status = string.Empty;


            string ClientId = ConfigurationSettings.AppSettings["strAppIDMsig"];
            string ClientToken = ConfigurationSettings.AppSettings["strHash"];
            string ClientCookies = ConfigurationSettings.AppSettings["strCookie"];
            string UrlWs = ConfigurationSettings.AppSettings["strUrlMSIG"];


            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient(UrlWs + "/authenservice/authen/gentoken");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", ClientCookies.ToString());

                var Arr_Body = new
                {
                    appId = ClientId.ToString(),
                    hashvalue = ClientToken.ToString(),

                };

                var body = JsonConvert.SerializeObject(Arr_Body);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                var RESULT_API_AUTH = serializer.Deserialize<M_GET_TOKEN_MSIG>(response.Content);

                token = RESULT_API_AUTH.result_data.token.ToString();
                Status = RESULT_API_AUTH.result_code.ToString();
                FN_KEEP_LOG(token.ToString(), Status, "MSIG");

            }
            catch (Exception Error)
            {
                token = "";
                Status = Error.Message.ToString();
                FN_KEEP_LOG(token.ToString(), Status, "MSIG");
            }
            finally
            {



            }

            return token;


        }

        [WebMethod]
        public string FN_VIB_GET_QUOTATION(string saleMethod, string carBrand, string carModel, string registrationYear, string vehicleTypeCode, string engineCC, string carWeight, string driversList, string startDate, string insuranceType, string repairType)
        {

            DateTime DT_NOW = DateTime.Now;
            string DF_TODATE = (Convert.ToInt32(DT_NOW.Year)).ToString() + "-" + DT_NOW.Month.ToString("00") + "-" + DT_NOW.Day.ToString("00") + "T" + DT_NOW.Hour.ToString("00") + ":" + DT_NOW.Minute.ToString("00") + ":" + DT_NOW.Second.ToString("00");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string token = FN_VIB_GEN_TOKEN();
            string STAUTS = ConfigurationManager.AppSettings["Status_APP"].ToString();
            var Culture = new CultureInfo("en-US");
            String[] s2 = new String[1];
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;
            var RESULT_GET_QUOTATION = new VIB_GET_QUOTATION();
            string INPUT_Body = string.Empty;
            string Response = string.Empty;
            var Result = new M_OUTPUT_MSIG();
            var ResultList = new List<M_OUTPUT_MSIG>();
            var Intput = new M_INPUT_MSIG();

            string Status = string.Empty;
            string ClientId = ConfigurationSettings.AppSettings["strClientIdVIB"];
            string ClientSecret = ConfigurationSettings.AppSettings["strClientSecretVIB"];
            string UrlWs = ConfigurationSettings.AppSettings["strUrlVIB"];
            string str_agentCodeVIB = ConfigurationSettings.AppSettings["str_agentCodeVIB"];
            string[] listvehicleTypeCode = vehicleTypeCode.Split(',');
            string[] listinsuranceType = (insuranceType == "") ? new string[] { "1", "2", "3", "4", "2P", "3P" } : insuranceType.Split(',');
            string[] listrepairType = (repairType == "") ? new string[] { "D", "G" } : repairType.Split(',')  ;
			string[] arraydriverlist = driversList.Split(',');
            var arrdriversList = new List<VIB_GET_QUOTATION__DRIVERSLIST>();
            //string[] arrdriversEm = new String[0];
            if (driversList == "" || driversList == " ")
            {


            }
            else
            {

                for (int i = 0; i < arraydriverlist.Length; i++)
                {
                    var vib_driversList = new VIB_GET_QUOTATION__DRIVERSLIST();
                    vib_driversList.insuredBirthDate = arraydriverlist[i].ToString();
                    arrdriversList.Add(vib_driversList);
                }

            }


            //if (driversList == "" && driversList == " ")
            //{
            //    arrdriversList[0] = string.Empty;
            //}
            //else {
            //    arrdriversList = arraydriverlist;
            //}



            try
            {
                var client = new RestClient(UrlWs + "/policy/motor/vmi/v1/quotation");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("sourceTransID", "t101");
                request.AddHeader("clientId", ClientId.ToString());
                request.AddHeader("clientSecret", ClientSecret.ToString());
                request.AddHeader("requestTime", DF_TODATE.ToString());
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("languagePreference", "TH");
                request.AddHeader("Authorization", "Bearer " + token.ToString());



                var arr_preference = new
                {
                    startDate = startDate,
                    insuranceType = listinsuranceType,
                    repairType = listrepairType

                };


                //var arrdriversList = new {


                //}


                var Arr_Body = new
                {
                    agentCode = str_agentCodeVIB,
                    saleMethod = saleMethod,
                    carBrand = carBrand,
                    carModel = carModel,
                    carSubModel = "default",
                    registrationYear = registrationYear,
                    vehicleTypeCode = listvehicleTypeCode,
                    engineCC = engineCC,
                    carWeight = carWeight,
                    driversList = arrdriversList,
                    preference = arr_preference,

                };
                INPUT_Body = JsonConvert.SerializeObject(Arr_Body);
                var body = JsonConvert.SerializeObject(Arr_Body);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);
                Response = response.Content;
                //if (Response.) {


                //}
                RESULT_GET_QUOTATION = serializer.Deserialize<VIB_GET_QUOTATION>(response.Content);
                FN_KEEP_LOG(body, response.Content, "VIB");




            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG(INPUT_Body, Response, "VIB");
                //FN_KEEP_LOG("", "", "");
            }
            finally
            {



            }

            return new JavaScriptSerializer().Serialize(RESULT_GET_QUOTATION);


        }

        public string FN_VIB_GEN_TOKEN()
        {

            DateTime DT_NOW = DateTime.Now;
            string DF_TODATE = (Convert.ToInt32(DT_NOW.Year)).ToString() + "-" + DT_NOW.Month.ToString("00") + "-" + DT_NOW.Day.ToString("00") + "T" + DT_NOW.Hour.ToString("00") + ":" + DT_NOW.Minute.ToString("00") + ":" + DT_NOW.Second.ToString("00");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;
            string token = string.Empty;
            string Status = string.Empty;
            string INPUT_Body = string.Empty;
            string Response = string.Empty;

            string ClientId = ConfigurationSettings.AppSettings["strClientIdVIB"];
            string ClientSecret = ConfigurationSettings.AppSettings["strClientSecretVIB"];
            string UrlWs = ConfigurationSettings.AppSettings["strUrlVIB"];
            string pass = ConfigurationSettings.AppSettings["strApi_passVIB"];

            try
            {
                var client = new RestClient(UrlWs + "/authen/token/v1/generate");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("sourceTransID", "t101");
                request.AddHeader("clientId", ClientId.ToString());
                request.AddHeader("clientSecret", ClientSecret.ToString());
                request.AddHeader("requestTime", DF_TODATE.ToString());
                request.AddHeader("languagePreference", "TH");
                request.AddHeader("grantType", "password");
                request.AddHeader("userName", "00000_uat");
                request.AddHeader("passWord", pass.ToString());
                request.AddHeader("scope", "profile");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                //var result_api = JsonConvert.DeserializeObject(response.Content);
                var RESULT_API_AUTH = serializer.Deserialize<VIB_GET_TOKEN>(response.Content);
                token = RESULT_API_AUTH.access_token;

            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG("", Status, "VIB");
            }
            finally
            {



            }

            return token;


        }

        [WebMethod]
        public string FN_KPI_GET_QUOTATION(string agentCode, string brand, string model, string carManufactureYear, string motorClass /*carInsuranceType*/)
        {
			brand = char.ToUpper(brand[0]) + brand.Substring(1).ToLower();
			model = char.ToUpper(model[0]) + model.Substring(1).ToLower();

			if (motorClass.Trim() == "" || motorClass.Trim() == "null")
			{
				motorClass = "1";
			}

			var Culture = new CultureInfo("en-US");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            //var Result = new KPI_GET_QUOTATION.PlanList();
            //var ResultList = new List<KPI_GET_QUOTATION.PlanList>();
            var Result = new KPI_GET_QUOTATION.PlanList();


            string Response = string.Empty;

            string Status = string.Empty;

            string strUrlKPI = ConfigurationSettings.AppSettings["strUrlKPI"];

            string strissue_api_key = ConfigurationSettings.AppSettings["strissue_api_key"];
            string strAuthorization = ConfigurationSettings.AppSettings["strAuthorization"];
            string strbroker_code = ConfigurationSettings.AppSettings["strbroker_code"];


            var body = "";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                var client = new RestClient(strUrlKPI + "/product_mo/get_plan");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", strAuthorization);

                var Arr_Body = new
                {

                    agentCode = strbroker_code,
                    brand = brand.ToString(),
                    model = model.ToString(),
                    carManufactureYear = carManufactureYear.ToString(),
                    motorClass = motorClass.ToString(),

                };

                //var bodyHash = JsonConvert.SerializeObject(Arr_Body);               

                body = JsonConvert.SerializeObject(Arr_Body);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                Status = "success";
                if (response.StatusCode.ToString() == "OK")
                {
					Response = response.Content;
					Result = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(response.Content);
				}
                else
				{
					string json = "{\"result\": {  \"resultCode\": \"404\",   \"resultMessage\": \"error\",   \"resultSize\": 0}}";
					Result = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(json);
				}
                //Result = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(response.Content);
                

            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG(body, Status, "KPI");
            }
            finally
            {



            }

            return new JavaScriptSerializer().Serialize(Result);


        }

        public string FN_KEEP_LOG(string INPUT_PARAMETER, string API_RESPONSE, string TYPE_INSURANCE)
        {


            CultureInfo ThaiCulture = new CultureInfo("th-TH");
            DataTable dtTable = new DataTable();

            SqlConnection Con = new SqlConnection();
            Con.ConnectionString = ConfigurationSettings.AppSettings["strConnSGDIRECT"]; //Table = WS_SGB_SAVE_LOG_INSURANCE//
            string Status = string.Empty;

            try
            {

                Con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandTimeout = 0;

                sqlCmd = new SqlCommand("SGB_ADD_LOG_DATA_INSURER", Con);
                sqlCmd.Parameters.AddWithValue("@INPUT_PARAMETER", INPUT_PARAMETER.ToString());
                sqlCmd.Parameters.AddWithValue("@API_RESPONSE", API_RESPONSE.ToString());
                sqlCmd.Parameters.AddWithValue("@TYPE_INSURANCE", TYPE_INSURANCE.ToString());
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.ExecuteNonQuery();

                Status = "SUCCESS";

            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();

            }
            finally
            {

                Con.Close();

            }
            return Status;
        }

        [WebMethod]
        public string FN_ALL_INSURER_GET_QUOTATION(string InsuranceCompany, string carBrand, string carModel, string carYear, string carRegisYear, string carNo, string carProvince, string carEngineCC, string carWeight, string carCamera, string carFixType, string carInsuranceType, string identifyDrivers, string driverName1, string driverName2, string driverBirthDate1, string driverBirthDate2, string Inception_date, string Expiry_date, string Number_Of_Seating)
        {



			  string txtRequest = "< carBrand > " + carBrand + " </ carBrand >" +
                                    "< carModel >  " + carModel + " </ carModel >" +
                                    "< carYear >  " + carYear + " </ carYear >" +
                                    "< carRegisYear >  " + carRegisYear + " </ carRegisYear >" +
                                    "< InsuranceCompany >  " + InsuranceCompany + " </ InsuranceCompany >" +
                                    "< carNo >  " + carNo + " </ carNo >" +
                                    "< carProvince >  " + carProvince + " </ carProvince >" +
                                    "< carEngineCC >  " + carEngineCC + " </ carEngineCC >" +
                                    "< carWeight >  " + carWeight + " </ carWeight >" +
                                    "< carCamera >  " + carCamera + " </ carCamera >" +
                                    "< carFixType >  " + carFixType + " </ carFixType >" +
                                    "< carInsuranceType >  " + carInsuranceType + " </ carInsuranceType >" +
                                    "< identifyDrivers >  " + identifyDrivers + " </ identifyDrivers >" +
                                    "< driverName1 > " + driverName1 + "</ driverName1 >" +
                                    "< driverName2 > " + driverName2 + "</ driverName2 >" +
                                    "< driverBirthDate1 > " + driverBirthDate1 + "</ driverBirthDate1 >" +
                                    "< driverBirthDate2 > " + driverBirthDate2 + "</ driverBirthDate2 >" +
                                    "< Inception_date >  " + Inception_date + " </ Inception_date >" +
                                    "< Expiry_date >  " + Expiry_date + " </ Expiry_date >" +
                                    "< Number_Of_Seating >  " + Number_Of_Seating + " </ Number_Of_Seating >";


			if (Inception_date.Trim() == "" || Inception_date.Trim() == "null" || Inception_date.Trim() == "0001-01-01")
			{
				Inception_date = "2022-03-01";
			}
			if (Expiry_date.Trim() == "" || Expiry_date.Trim() == "null" || Inception_date.Trim() == "0001-01-01")
			{
				Expiry_date = "2023-03-01";
			}

			carBrand = char.ToUpper(carBrand[0]) + carBrand.Substring(1).ToLower();
			carModel = char.ToUpper(carModel[0]) + carModel.Substring(1).ToLower();

			string MSIG_IdentifyDriver = string.Empty;
            string FLAG_CCTV = string.Empty;
            var dejsResult_VIB = new VIB_GET_QUOTATION();
            var dejsResult_JMI = new M_OUTPUT_JMI();
            var dejsResult_MSIG = new M_OUTPUT_MSIG();

            //bendcemb เริ่ม//
            var dejsResult_KPI = new KPI_GET_QUOTATION.PlanList();

            var Result_ALL_QUOTATION = new M_OUTPUT_ALL_INSURANCE();

            string list_driversList = string.Empty;
            if (driverBirthDate1 != "" && driverBirthDate2 != "")
            {

                list_driversList = driverBirthDate1 + "," + driverBirthDate2;

            }
            else if (driverBirthDate1 != "")
            {

                list_driversList = driverBirthDate1;

            }
            else if (driverBirthDate2 != "")
            {

                list_driversList = driverBirthDate2;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var ResultList_QUOTATION = new List<M_OUTPUT_ALL_INSURANCE_DATA>();

            try
            {

				


				//ค่าเริ่มต้น
				if (InsuranceCompany.ToString() == "")
                {


                    //serializer.Deserialize<VIB_GET_TOKEN>(response.Content);
                    var Result_VIB_GET_QUOTATION = FN_VIB_GET_QUOTATION(" ", carBrand.ToUpper(), carModel.ToUpper(), carRegisYear, carNo, "", "", list_driversList, "", carInsuranceType, carFixType);
                    dejsResult_VIB = serializer.Deserialize<VIB_GET_QUOTATION>(Result_VIB_GET_QUOTATION);
                    if (dejsResult_VIB.integrationStatusCode == "0")
                    {
                        for (int i = 0; i < dejsResult_VIB.data.Count(); i++)
                        {
                            var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();

                            
                            OUT_GET_QUOTATION.priceListCode = dejsResult_VIB.data[i].packageCode.ToString();
                            OUT_GET_QUOTATION.priceListName = dejsResult_VIB.data[i].packageName.ToString();
                            OUT_GET_QUOTATION.insuranceCompany = "VIB";
                            OUT_GET_QUOTATION.carNo = dejsResult_VIB.data[i].vehicleTypeCode.ToString();
                            OUT_GET_QUOTATION.carBrand = dejsResult_VIB.data[i].carBrand.ToString();
                            OUT_GET_QUOTATION.carModel = dejsResult_VIB.data[i].carModel.ToString();
                            OUT_GET_QUOTATION.carEngineCC = dejsResult_VIB.data[i].engineCC.ToString();
                            OUT_GET_QUOTATION.carRegisYear = dejsResult_VIB.data[i].registrationYear.ToString();

                            if (dejsResult_VIB.data[i].repairType.ToString() == "G")
                            {
                                OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";

                            }
                            else if (dejsResult_VIB.data[i].repairType.ToString() == "D")
                            {
                                OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
                            }


							if (dejsResult_VIB.data[i].insuranceType.ToString() == "1")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 1";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "2")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "3")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 3";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "4")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 4";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "2P")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 2+";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "3P")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 3+";
							}

							//OUT_GET_QUOTATION.carFixType = dejsResult_VIB.data[i].repairType.ToString();

							/*OUT_GET_QUOTATION.carInsuranceType = dejsResult_VIB.data[i].insuranceType.ToString();*/
                            OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_VIB.data[i].ownDamage.sumInsured.ToString();
                            OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_VIB.data[i].netPremium.ToString();
                            OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_VIB.data[i].stamp.ToString();
                            OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_VIB.data[i].vat.ToString();
                            OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_VIB.data[i].totalPremium.ToString();
                            OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_VIB.data[i].liability.tpbiPerPerson.ToString();
                            OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_VIB.data[i].liability.tpbiPerEvent.ToString();
                            OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_VIB.data[i].liability.tppdPerEvent.ToString();
                            OUT_GET_QUOTATION.thieftAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
                            OUT_GET_QUOTATION.fireAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
                            OUT_GET_QUOTATION.floodAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
                            OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_VIB.data[i].ownDamage.deductible.ToString();
                            if (dejsResult_VIB.data[i].additionalCoverage.Count != 0)
                            {
                                OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.driverDeath.ToString();
                                OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.driverDeath.ToString();
                                OUT_GET_QUOTATION.accidentDrive = null;
                                OUT_GET_QUOTATION.acidentPassenger = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.numberPassengerTemporaryDisability.ToString();
                                OUT_GET_QUOTATION.disibilityAMT = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.temporaryDisabilityPassenger.ToString();
                                OUT_GET_QUOTATION.disibilityDriver = null;
                                OUT_GET_QUOTATION.disibilityPassenger = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.numberPassengerTemporaryDisability.ToString();
                                OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_VIB.data[i].additionalCoverage[0].medicalExpense.ToString();
                                OUT_GET_QUOTATION.bailBondAMT = dejsResult_VIB.data[i].additionalCoverage[0].bailBond.ToString();
                            }
                            else
                            {
                                OUT_GET_QUOTATION.personalAccidentAMT = null;
                                OUT_GET_QUOTATION.personalAccidentAMT = null;
                                OUT_GET_QUOTATION.accidentDrive = null;
                                OUT_GET_QUOTATION.acidentPassenger = null;
                                OUT_GET_QUOTATION.disibilityAMT = null;
                                OUT_GET_QUOTATION.disibilityDriver = null;
                                OUT_GET_QUOTATION.disibilityPassenger = null;
                                OUT_GET_QUOTATION.medicalExpenseAMT = null;
                                OUT_GET_QUOTATION.bailBondAMT = null;
                            }

                            OUT_GET_QUOTATION.effectiveDate = null;
                            OUT_GET_QUOTATION.expireDate = null;
                            ResultList_QUOTATION.Add(OUT_GET_QUOTATION);

                        }

                    }

                    var Result_JMI_GET_QUOTATION = FN_JMI_GET_QUOTATION("MOTOR_PACKAGE_SGB", carBrand, carModel, carYear, carEngineCC, carNo);
                    dejsResult_JMI = serializer.Deserialize<M_OUTPUT_JMI>(Result_JMI_GET_QUOTATION);
                    if (dejsResult_JMI.status == "200")
                    {

                        for (int i = 0; i < dejsResult_JMI.data.Count; i++)
                        {
                            var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();
                            OUT_GET_QUOTATION.priceListCode = dejsResult_JMI.data[i].Package_code.ToString();
                            OUT_GET_QUOTATION.priceListName = dejsResult_JMI.data[i].Package_name.ToString();
                            OUT_GET_QUOTATION.insuranceCompany = "JMI";
                            OUT_GET_QUOTATION.carNo = null;
                            OUT_GET_QUOTATION.carBrand = null;
                            OUT_GET_QUOTATION.carModel = null;
                            OUT_GET_QUOTATION.carEngineCC = null;
                            OUT_GET_QUOTATION.carRegisYear = null;
                            OUT_GET_QUOTATION.carFixType = dejsResult_JMI.data[i].Type_garage.ToString();
                            OUT_GET_QUOTATION.carInsuranceType = dejsResult_JMI.data[i].Insurance_type.ToString();
							OUT_GET_QUOTATION.carInsuranceTypeName = dejsResult_JMI.data[i].Insurance_type.ToString();
							OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_JMI.data[i].Sum_insure.ToString();
                            OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_JMI.data[i].Premium.ToString();
                            OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_JMI.data[i].Duty.ToString();
                            OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_JMI.data[i].Vat.ToString();
                            OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_JMI.data[i].Total_insurance.ToString();
                            OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_JMI.data[i].Damage_life_per_person.ToString();
                            OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_JMI.data[i].Damage_life_per_time.ToString();
                            OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_JMI.data[i].Damage_property.ToString();
                            OUT_GET_QUOTATION.thieftAMT = dejsResult_JMI.data[i].Damage_lost.ToString();
                            OUT_GET_QUOTATION.fireAMT = dejsResult_JMI.data[i].Damage_fire.ToString();
                            OUT_GET_QUOTATION.floodAMT = dejsResult_JMI.data[i].Damage_flood.ToString();
                            OUT_GET_QUOTATION.carDecitibleAMT = null;
                            OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_JMI.data[i].Damage_death_permanent_disability.ToString();
                            OUT_GET_QUOTATION.accidentDrive = dejsResult_JMI.data[i].Coverage_amount_driver.ToString();
                            OUT_GET_QUOTATION.acidentPassenger = dejsResult_JMI.data[i].Coverage_amount_passengers.ToString();
                            OUT_GET_QUOTATION.disibilityAMT = dejsResult_JMI.data[i].Damage_temporary_disability.ToString();
                            OUT_GET_QUOTATION.disibilityDriver = dejsResult_JMI.data[i].Coverage_amount_driver.ToString();
                            OUT_GET_QUOTATION.disibilityPassenger = dejsResult_JMI.data[i].Coverage_amount_passengers.ToString();
                            OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_JMI.data[i].Medical_expenses.ToString();
                            OUT_GET_QUOTATION.bailBondAMT = dejsResult_JMI.data[i].Driver_bail.ToString();
                            OUT_GET_QUOTATION.effectiveDate = null;
                            OUT_GET_QUOTATION.expireDate = null;
                            ResultList_QUOTATION.Add(OUT_GET_QUOTATION);


                        }
                    }




					char[] delimiters = { '-' };
					string[] DfInception_date = Inception_date.Split(delimiters, StringSplitOptions.None);
					string SR_Inception_date = DfInception_date[0].ToString() + DfInception_date[1].ToString() + DfInception_date[2].ToString();

					/*DateTime DfExpiry_date = Convert.ToDateTime(Expiry_date);*/
					string[] DfExpiry_date = Expiry_date.Split(delimiters, StringSplitOptions.None);
					string SR_Expiry_date = DfExpiry_date[0].ToString() + DfExpiry_date[1].ToString() + DfExpiry_date[2].ToString();

					if (identifyDrivers == "true")
                    {

                        var length_listdrivers = list_driversList.Split(',');
                        MSIG_IdentifyDriver = length_listdrivers.Length.ToString();


                    }
                    else if (identifyDrivers == "false")
                    {

                        MSIG_IdentifyDriver = "0";

                    }
                    if (carCamera == "true")
                    {

                        FLAG_CCTV = "Y";

                    }
                    else if (carCamera == "false")
                    {
                        FLAG_CCTV = "N";

                    }

                    var Result_MSIG_GET_QUOTATION = FN_MSIG_GET_QUOTATION("R", carBrand, carModel, "", "", SR_Inception_date, SR_Expiry_date, MSIG_IdentifyDriver, "N", carNo.ToString(), "", "", "WS-SGB", carRegisYear, 52000000, 52000000, "01", "BCA8292", "P", FLAG_CCTV, "", 0, Number_Of_Seating, carEngineCC, 0);
                    dejsResult_MSIG = serializer.Deserialize<M_OUTPUT_MSIG>(Result_MSIG_GET_QUOTATION);
                    if (dejsResult_MSIG.success.ToLower() == "true")
                    {
                        for (int i = 0; i < dejsResult_MSIG.vehPrem.Count; i++)
                        {
                            var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();
                            OUT_GET_QUOTATION.priceListCode = dejsResult_MSIG.vehPrem[i].packagecode.ToString();
                            OUT_GET_QUOTATION.priceListName = dejsResult_MSIG.vehPrem[i].plan.ToString();
                            OUT_GET_QUOTATION.insuranceCompany = "MSIG";
                            OUT_GET_QUOTATION.carNo = null;
                            OUT_GET_QUOTATION.carBrand = null;
                            OUT_GET_QUOTATION.carModel = null;
                            OUT_GET_QUOTATION.carEngineCC = dejsResult_MSIG.vehPrem[i].cc.ToString();
                            OUT_GET_QUOTATION.carRegisYear = null;

                            if (dejsResult_MSIG.vehPrem[i].cc.ToString() == "A001")
                            {
                                OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";

                            }
                            else if (dejsResult_MSIG.vehPrem[i].cc.ToString() == "D001")
                            {
                                OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
                            }


							if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "1")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 1";
							}
							else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "2")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2";
							}
							else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "3")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 3";
							}
							else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "2+")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2+";
							}
							else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "3+")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 3+";
							}

							OUT_GET_QUOTATION.carFixType = dejsResult_MSIG.vehPrem[i].cmp_flag.ToString();
                           /* OUT_GET_QUOTATION.carInsuranceType = dejsResult_MSIG.vehPrem[i].cmp_flag.ToString();*/
                            OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_MSIG.vehPrem[i].suminsured.ToString();
                            OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_MSIG.vehPrem[i].vmi_prem_gross_amount.ToString();
                            OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_stamp_amount.ToString();
                            OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_vat_amount.ToString();
                            OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_prem_due_amount.ToString();
                            OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_MSIG.vehPrem[i].tpbiperperson.ToString();
                            OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_MSIG.vehPrem[i].tpbiperevent.ToString();
                            OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_MSIG.vehPrem[i].tppd.ToString();
                            OUT_GET_QUOTATION.thieftAMT = dejsResult_MSIG.vehPrem[i].tlamt.ToString();
                            OUT_GET_QUOTATION.fireAMT = dejsResult_MSIG.vehPrem[i].fiamt.ToString();
                            OUT_GET_QUOTATION.floodAMT = dejsResult_MSIG.vehPrem[i].flamt.ToString();
                            OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_MSIG.vehPrem[i].txamt.ToString();
                            OUT_GET_QUOTATION.personalAccidentAMT = null;
                            OUT_GET_QUOTATION.accidentDrive = null;
                            OUT_GET_QUOTATION.acidentPassenger = null;
                            OUT_GET_QUOTATION.disibilityAMT = null;
                            OUT_GET_QUOTATION.disibilityDriver = null;
                            OUT_GET_QUOTATION.disibilityPassenger = dejsResult_MSIG.vehPrem[i].ry01compenpassenger.ToString();
                            OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_MSIG.vehPrem[i].ry02.ToString();
                            OUT_GET_QUOTATION.bailBondAMT = dejsResult_MSIG.vehPrem[i].ry03.ToString();
                            OUT_GET_QUOTATION.effectiveDate = dejsResult_MSIG.vehPrem[i].effdate.ToString();
                            OUT_GET_QUOTATION.expireDate = dejsResult_MSIG.vehPrem[i].expdate.ToString();
                            ResultList_QUOTATION.Add(OUT_GET_QUOTATION);

                        }
                    }


                    //KPI_GET_QUOTATION//
                    var Result_KPI_GET_QUOTATION = FN_KPI_GET_QUOTATION("0032003939", carBrand, carModel, carYear, carInsuranceType);
                    dejsResult_KPI = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(Result_KPI_GET_QUOTATION);
                    if (dejsResult_KPI.result.resultMessage == "success")
                    {

                        for (int i = 0; i < dejsResult_KPI.plan.Count; i++)
                        {
                            var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();

                            OUT_GET_QUOTATION.priceListName = dejsResult_KPI.plan[i].planNameTh.ToString();
                            OUT_GET_QUOTATION.insuranceCompany = "KPI";

                            //planlist//
                            if (dejsResult_KPI.plan[i].plan.Count != 0)
                            {
                                OUT_GET_QUOTATION.priceListCode = dejsResult_KPI.plan[i].si.ToString();

                                OUT_GET_QUOTATION.carNo = dejsResult_KPI.plan[i].plan[0].vehicleCode.ToString();

                                OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
                                OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_KPI.plan[i].plan[0].netPrem.ToString();
                                OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_KPI.plan[i].plan[0].stamp.ToString();
                                OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_KPI.plan[i].plan[0].vat.ToString();
                                OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_KPI.plan[i].plan[0].totalPrem.ToString();

                                OUT_GET_QUOTATION.effectiveDate = dejsResult_KPI.plan[i].plan[0].effectiveDate.ToString();
                                OUT_GET_QUOTATION.expireDate = dejsResult_KPI.plan[i].plan[0].expiryDate.ToString();

                                OUT_GET_QUOTATION.thieftAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
                                OUT_GET_QUOTATION.fireAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
                                OUT_GET_QUOTATION.floodAMT = null;
                            }
                            else
                            {
                                OUT_GET_QUOTATION.priceListCode = null;
                                OUT_GET_QUOTATION.carNo = null;

                                OUT_GET_QUOTATION.personalAccidentAMT = null;
                                OUT_GET_QUOTATION.premiumInsuranceAMT = null;
                                OUT_GET_QUOTATION.stampInsuranceTotal = null;
                                OUT_GET_QUOTATION.vatInsuranceTotal = null;
                                OUT_GET_QUOTATION.premiumInsuranceTotal = null;

                                OUT_GET_QUOTATION.effectiveDate = null;
                                OUT_GET_QUOTATION.expireDate = null;

                                OUT_GET_QUOTATION.thieftAMT = null;
                                OUT_GET_QUOTATION.fireAMT = null;
                            }

                            OUT_GET_QUOTATION.carBrand = dejsResult_KPI.plan[i].brand.ToString();
                            OUT_GET_QUOTATION.carModel = dejsResult_KPI.plan[i].model.ToString();
                            OUT_GET_QUOTATION.carEngineCC = dejsResult_KPI.plan[i].engineCapacity.ToString();
                            OUT_GET_QUOTATION.carRegisYear = dejsResult_KPI.plan[i].carManufactureYear.ToString();

                            if (dejsResult_KPI.plan[i].repairOption.ToString() == "G")
                            {
                                OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";
                            }
                            else if (dejsResult_KPI.plan[i].repairOption.ToString() == "D")
                            {
                                OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
                            }


                            //OUT_GET_QUOTATION.carFixType = dejsResult_KPI.plan[i].repairOption.ToString();

                            OUT_GET_QUOTATION.carInsuranceType = dejsResult_KPI.plan[i].motorClass.ToString();

                            //CoverageList
                            if (dejsResult_KPI.plan[i].coverageList.Count != 0)
                            {
                                if (dejsResult_KPI.plan[i].motorClass.ToString() == "1")
                                {
                                    OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_KPI.plan[i].coverageList[3].coverageAmt.ToString();
                                    OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_KPI.plan[i].coverageList[4].coverageAmt.ToString();
                                    OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_KPI.plan[i].coverageList[5].coverageAmt.ToString();

                                }
                                else
                                {
                                    OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_KPI.plan[i].coverageList[4].coverageAmt.ToString();
                                    OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_KPI.plan[i].coverageList[5].coverageAmt.ToString();
                                    OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_KPI.plan[i].coverageList[6].coverageAmt.ToString();
                                }

                                OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_KPI.plan[i].coverageList[1].coverageAmt.ToString();
                                OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_KPI.plan[i].coverageList[7].coverageAmt.ToString();
                                OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_KPI.plan[i].coverageList[8].coverageAmt.ToString();
                                OUT_GET_QUOTATION.bailBondAMT = dejsResult_KPI.plan[i].coverageList[9].coverageAmt.ToString();
                            }
                            else
                            {
                                OUT_GET_QUOTATION.bodyPersonAMT = null;
                                OUT_GET_QUOTATION.accidentPersonAMT = null;
                                OUT_GET_QUOTATION.propertiesPersonAMT = null;


                                OUT_GET_QUOTATION.carDecitibleAMT = null;
                                OUT_GET_QUOTATION.personalAccidentAMT = null;

                                OUT_GET_QUOTATION.medicalExpenseAMT = null;
                                OUT_GET_QUOTATION.bailBondAMT = null;
                            }



                            OUT_GET_QUOTATION.floodAMT = null;
                            OUT_GET_QUOTATION.accidentDrive = null;
                            OUT_GET_QUOTATION.acidentPassenger = null;
                            OUT_GET_QUOTATION.disibilityAMT = null;
                            OUT_GET_QUOTATION.disibilityDriver = null;
                            OUT_GET_QUOTATION.disibilityPassenger = null;

                            ResultList_QUOTATION.Add(OUT_GET_QUOTATION);


                        }
                    }
                }
                else
                {
					string input = InsuranceCompany.ToString();
					char[] delimiter = { ',' };

					string[] InsuranceCompanyList = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

					string VIB = "VIB";
					bool VIBExists = Array.IndexOf(InsuranceCompanyList, VIB) != -1;
					if (VIBExists)
					{
						var Result_VIB_GET_QUOTATION = FN_VIB_GET_QUOTATION(" ", carBrand.ToUpper(), carModel.ToUpper(), carRegisYear, carNo, "", "", list_driversList, "", carInsuranceType, carFixType);
						dejsResult_VIB = serializer.Deserialize<VIB_GET_QUOTATION>(Result_VIB_GET_QUOTATION);
						if (dejsResult_VIB.integrationStatusCode == "0")
						{
							for (int i = 0; i < dejsResult_VIB.data.Count(); i++)
							{
								var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();


								OUT_GET_QUOTATION.priceListCode = dejsResult_VIB.data[i].packageCode.ToString();
								OUT_GET_QUOTATION.priceListName = dejsResult_VIB.data[i].packageName.ToString();
								OUT_GET_QUOTATION.insuranceCompany = "VIB";
								OUT_GET_QUOTATION.carNo = dejsResult_VIB.data[i].vehicleTypeCode.ToString();
								OUT_GET_QUOTATION.carBrand = dejsResult_VIB.data[i].carBrand.ToString();
								OUT_GET_QUOTATION.carModel = dejsResult_VIB.data[i].carModel.ToString();
								OUT_GET_QUOTATION.carEngineCC = dejsResult_VIB.data[i].engineCC.ToString();
								OUT_GET_QUOTATION.carRegisYear = dejsResult_VIB.data[i].registrationYear.ToString();

								if (dejsResult_VIB.data[i].repairType.ToString() == "G")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";

								}
								else if (dejsResult_VIB.data[i].repairType.ToString() == "D")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
								}


								if (dejsResult_VIB.data[i].insuranceType.ToString() == "1")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 1";
								}
								else if (dejsResult_VIB.data[i].insuranceType.ToString() == "2")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2";
								}
								else if (dejsResult_VIB.data[i].insuranceType.ToString() == "3")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 3";
								}
								else if (dejsResult_VIB.data[i].insuranceType.ToString() == "4")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 4";
								}
								else if (dejsResult_VIB.data[i].insuranceType.ToString() == "2P")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 2+";
								}
								else if (dejsResult_VIB.data[i].insuranceType.ToString() == "3P")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 3+";
								}

								//OUT_GET_QUOTATION.carFixType = dejsResult_VIB.data[i].repairType.ToString();

								//OUT_GET_QUOTATION.carInsuranceType = dejsResult_VIB.data[i].insuranceType.ToString();
								OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_VIB.data[i].ownDamage.sumInsured.ToString();
								OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_VIB.data[i].netPremium.ToString();
								OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_VIB.data[i].stamp.ToString();
								OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_VIB.data[i].vat.ToString();
								OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_VIB.data[i].totalPremium.ToString();
								OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_VIB.data[i].liability.tpbiPerPerson.ToString();
								OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_VIB.data[i].liability.tpbiPerEvent.ToString();
								OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_VIB.data[i].liability.tppdPerEvent.ToString();
								OUT_GET_QUOTATION.thieftAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
								OUT_GET_QUOTATION.fireAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
								OUT_GET_QUOTATION.floodAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
								OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_VIB.data[i].ownDamage.deductible.ToString();
								if (dejsResult_VIB.data[i].additionalCoverage.Count != 0)
								{
									OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.driverDeath.ToString();
									OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.driverDeath.ToString();
									OUT_GET_QUOTATION.accidentDrive = null;
									OUT_GET_QUOTATION.acidentPassenger = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.numberPassengerTemporaryDisability.ToString();
									OUT_GET_QUOTATION.disibilityAMT = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.temporaryDisabilityPassenger.ToString();
									OUT_GET_QUOTATION.disibilityDriver = null;
									OUT_GET_QUOTATION.disibilityPassenger = dejsResult_VIB.data[i].additionalCoverage[0].personAccident.numberPassengerTemporaryDisability.ToString();
									OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_VIB.data[i].additionalCoverage[0].medicalExpense.ToString();
									OUT_GET_QUOTATION.bailBondAMT = dejsResult_VIB.data[i].additionalCoverage[0].bailBond.ToString();
								}
								else
								{
									OUT_GET_QUOTATION.personalAccidentAMT = null;
									OUT_GET_QUOTATION.personalAccidentAMT = null;
									OUT_GET_QUOTATION.accidentDrive = null;
									OUT_GET_QUOTATION.acidentPassenger = null;
									OUT_GET_QUOTATION.disibilityAMT = null;
									OUT_GET_QUOTATION.disibilityDriver = null;
									OUT_GET_QUOTATION.disibilityPassenger = null;
									OUT_GET_QUOTATION.medicalExpenseAMT = null;
									OUT_GET_QUOTATION.bailBondAMT = null;
								}

								OUT_GET_QUOTATION.effectiveDate = null;
								OUT_GET_QUOTATION.expireDate = null;
								ResultList_QUOTATION.Add(OUT_GET_QUOTATION);

							}

						}
					}

					string JMI = "JMI";
					bool JMIExists = Array.IndexOf(InsuranceCompanyList, JMI) != -1;
					if (JMIExists)
					{
						var Result_JMI_GET_QUOTATION = FN_JMI_GET_QUOTATION("MOTOR_PACKAGE_SGB", carBrand, carModel, carYear, carEngineCC, carNo);
						dejsResult_JMI = serializer.Deserialize<M_OUTPUT_JMI>(Result_JMI_GET_QUOTATION);
						if (dejsResult_JMI.status == "200")
						{

							for (int i = 0; i < dejsResult_JMI.data.Count; i++)
							{
								var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();
								OUT_GET_QUOTATION.priceListCode = dejsResult_JMI.data[i].Package_code.ToString();
								OUT_GET_QUOTATION.priceListName = dejsResult_JMI.data[i].Package_name.ToString();
								OUT_GET_QUOTATION.insuranceCompany = "JMI";
								OUT_GET_QUOTATION.carNo = null;
								OUT_GET_QUOTATION.carBrand = null;
								OUT_GET_QUOTATION.carModel = null;
								OUT_GET_QUOTATION.carEngineCC = null;
								OUT_GET_QUOTATION.carRegisYear = null;
								OUT_GET_QUOTATION.carFixType = dejsResult_JMI.data[i].Type_garage.ToString();
								OUT_GET_QUOTATION.carInsuranceType = dejsResult_JMI.data[i].Insurance_type.ToString();

								OUT_GET_QUOTATION.carInsuranceTypeName = dejsResult_JMI.data[i].Insurance_type.ToString();

								OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_JMI.data[i].Sum_insure.ToString();
								OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_JMI.data[i].Premium.ToString();
								OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_JMI.data[i].Duty.ToString();
								OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_JMI.data[i].Vat.ToString();
								OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_JMI.data[i].Total_insurance.ToString();
								OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_JMI.data[i].Damage_life_per_person.ToString();
								OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_JMI.data[i].Damage_life_per_time.ToString();
								OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_JMI.data[i].Damage_property.ToString();
								OUT_GET_QUOTATION.thieftAMT = dejsResult_JMI.data[i].Damage_lost.ToString();
								OUT_GET_QUOTATION.fireAMT = dejsResult_JMI.data[i].Damage_fire.ToString();
								OUT_GET_QUOTATION.floodAMT = dejsResult_JMI.data[i].Damage_flood.ToString();
								OUT_GET_QUOTATION.carDecitibleAMT = null;
								OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_JMI.data[i].Damage_death_permanent_disability.ToString();
								OUT_GET_QUOTATION.accidentDrive = dejsResult_JMI.data[i].Coverage_amount_driver.ToString();
								OUT_GET_QUOTATION.acidentPassenger = dejsResult_JMI.data[i].Coverage_amount_passengers.ToString();
								OUT_GET_QUOTATION.disibilityAMT = dejsResult_JMI.data[i].Damage_temporary_disability.ToString();
								OUT_GET_QUOTATION.disibilityDriver = dejsResult_JMI.data[i].Coverage_amount_driver.ToString();
								OUT_GET_QUOTATION.disibilityPassenger = dejsResult_JMI.data[i].Coverage_amount_passengers.ToString();
								OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_JMI.data[i].Medical_expenses.ToString();
								OUT_GET_QUOTATION.bailBondAMT = dejsResult_JMI.data[i].Driver_bail.ToString();
								OUT_GET_QUOTATION.effectiveDate = null;
								OUT_GET_QUOTATION.expireDate = null;
								ResultList_QUOTATION.Add(OUT_GET_QUOTATION);


							}
						}
					}

					string KPI = "KPI";
					bool KPIExists = Array.IndexOf(InsuranceCompanyList, KPI) != -1;
					if (KPIExists)
					{
						var Result_KPI_GET_QUOTATION = FN_KPI_GET_QUOTATION("0032003939", carBrand, carModel, carYear, carInsuranceType);
						dejsResult_KPI = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(Result_KPI_GET_QUOTATION);
						if (dejsResult_KPI.result.resultMessage == "success")
						{

							for (int i = 0; i < dejsResult_KPI.plan.Count; i++)
							{
								var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();

								OUT_GET_QUOTATION.priceListName = dejsResult_KPI.plan[i].planNameTh.ToString();
								OUT_GET_QUOTATION.insuranceCompany = "KPI";

								//planlist//
								if (dejsResult_KPI.plan[i].plan.Count != 0)
								{
									OUT_GET_QUOTATION.priceListCode = dejsResult_KPI.plan[i].si.ToString();

									OUT_GET_QUOTATION.carNo = dejsResult_KPI.plan[i].plan[0].vehicleCode.ToString();

									OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
									OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_KPI.plan[i].plan[0].netPrem.ToString();
									OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_KPI.plan[i].plan[0].stamp.ToString();
									OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_KPI.plan[i].plan[0].vat.ToString();
									OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_KPI.plan[i].plan[0].totalPrem.ToString();

									OUT_GET_QUOTATION.effectiveDate = dejsResult_KPI.plan[i].plan[0].effectiveDate.ToString();
									OUT_GET_QUOTATION.expireDate = dejsResult_KPI.plan[i].plan[0].expiryDate.ToString();

									OUT_GET_QUOTATION.thieftAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
									OUT_GET_QUOTATION.fireAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
									OUT_GET_QUOTATION.floodAMT = null;
								}
								else
								{
									OUT_GET_QUOTATION.priceListCode = null;
									OUT_GET_QUOTATION.carNo = null;

									OUT_GET_QUOTATION.personalAccidentAMT = null;
									OUT_GET_QUOTATION.premiumInsuranceAMT = null;
									OUT_GET_QUOTATION.stampInsuranceTotal = null;
									OUT_GET_QUOTATION.vatInsuranceTotal = null;
									OUT_GET_QUOTATION.premiumInsuranceTotal = null;

									OUT_GET_QUOTATION.effectiveDate = null;
									OUT_GET_QUOTATION.expireDate = null;

									OUT_GET_QUOTATION.thieftAMT = null;
									OUT_GET_QUOTATION.fireAMT = null;
								}


								OUT_GET_QUOTATION.carBrand = dejsResult_KPI.plan[i].brand.ToString();
								OUT_GET_QUOTATION.carModel = dejsResult_KPI.plan[i].model.ToString();
								OUT_GET_QUOTATION.carEngineCC = dejsResult_KPI.plan[i].engineCapacity.ToString();
								OUT_GET_QUOTATION.carRegisYear = dejsResult_KPI.plan[i].carManufactureYear.ToString();


								if (dejsResult_KPI.plan[i].repairOption.ToString() == "G")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";
								}
								else if (dejsResult_KPI.plan[i].repairOption.ToString() == "D")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
								}


								//OUT_GET_QUOTATION.carFixType = dejsResult_KPI.plan[i].repairOption.ToString();

								OUT_GET_QUOTATION.carInsuranceType = dejsResult_KPI.plan[i].motorClass.ToString();

								//CoverageList
								if (dejsResult_KPI.plan[i].coverageList.Count != 0)
								{
									if (dejsResult_KPI.plan[i].motorClass.ToString() == "1")
									{
										OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_KPI.plan[i].coverageList[3].coverageAmt.ToString();
										OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_KPI.plan[i].coverageList[4].coverageAmt.ToString();
										OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_KPI.plan[i].coverageList[5].coverageAmt.ToString();

									}
									else
									{
										OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_KPI.plan[i].coverageList[4].coverageAmt.ToString();
										OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_KPI.plan[i].coverageList[5].coverageAmt.ToString();
										OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_KPI.plan[i].coverageList[6].coverageAmt.ToString();
									}

									OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_KPI.plan[i].coverageList[1].coverageAmt.ToString();
									OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_KPI.plan[i].coverageList[7].coverageAmt.ToString();
									OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_KPI.plan[i].coverageList[8].coverageAmt.ToString();
									OUT_GET_QUOTATION.bailBondAMT = dejsResult_KPI.plan[i].coverageList[9].coverageAmt.ToString();
								}
								else
								{
									OUT_GET_QUOTATION.bodyPersonAMT = null;
									OUT_GET_QUOTATION.accidentPersonAMT = null;
									OUT_GET_QUOTATION.propertiesPersonAMT = null;


									OUT_GET_QUOTATION.carDecitibleAMT = null;
									OUT_GET_QUOTATION.personalAccidentAMT = null;

									OUT_GET_QUOTATION.medicalExpenseAMT = null;
									OUT_GET_QUOTATION.bailBondAMT = null;
								}



								OUT_GET_QUOTATION.floodAMT = null;
								OUT_GET_QUOTATION.accidentDrive = null;
								OUT_GET_QUOTATION.acidentPassenger = null;
								OUT_GET_QUOTATION.disibilityAMT = null;
								OUT_GET_QUOTATION.disibilityDriver = null;
								OUT_GET_QUOTATION.disibilityPassenger = null;

								ResultList_QUOTATION.Add(OUT_GET_QUOTATION);


							}
						}
					}

					string MSIG = "MSIG";
					bool MSIGExists = Array.IndexOf(InsuranceCompanyList, MSIG) != -1;
					if (MSIGExists)
					{
						/*DateTime DfInception_date = Convert.ToDateTime(Inception_date);*/
						
						char[] delimiters = { '-' };
						string[] DfInception_date = Inception_date.Split(delimiters, StringSplitOptions.None);
						string SR_Inception_date = DfInception_date[0].ToString() + DfInception_date[1].ToString() + DfInception_date[2].ToString();

						/*DateTime DfExpiry_date = Convert.ToDateTime(Expiry_date);*/
						string[] DfExpiry_date = Expiry_date.Split(delimiters, StringSplitOptions.None);
						string SR_Expiry_date = DfExpiry_date[0].ToString() + DfExpiry_date[1].ToString() + DfExpiry_date[2].ToString();

						if (identifyDrivers.ToLower() == "true")
						{

							var length_listdrivers = list_driversList.Split(',');
							MSIG_IdentifyDriver = length_listdrivers.Length.ToString();


						}
						else if (identifyDrivers.ToLower() == "false")
						{
							MSIG_IdentifyDriver = "0";
						}
						if (carCamera.ToLower() == "true")
						{
							FLAG_CCTV = "Y";
						}
						else if (carCamera.ToLower() == "false")
						{
							FLAG_CCTV = "N";

						}


                        /*var Result_MSIG_GET_QUOTATION = FN_MSIG_GET_QUOTATION("R", "Honda", "Jazz", "", "", "20220330", "20230330", "0", "N", "110", "", "", "WS-SGB", "2021", 52000000, 52000000, "01", "BCA8292", "P", "N", "", 0, "7", "1500", 0);*/

                        var Result_MSIG_GET_QUOTATION = FN_MSIG_GET_QUOTATION("R", carBrand, carModel, "", "", SR_Inception_date, SR_Expiry_date, MSIG_IdentifyDriver, "N", carNo.ToString(), "", "", "WS-SGB", carRegisYear, 52000000, 52000000, "01", "BCA8292", "P", FLAG_CCTV, "", 0, Number_Of_Seating, carEngineCC, 0);
                        dejsResult_MSIG = serializer.Deserialize<M_OUTPUT_MSIG>(Result_MSIG_GET_QUOTATION);
						if (dejsResult_MSIG.success.ToLower() == "true")
						{
							for (int i = 0; i < dejsResult_MSIG.vehPrem.Count; i++)
							{
								var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();
								OUT_GET_QUOTATION.priceListCode = dejsResult_MSIG.vehPrem[i].packagecode.ToString();
								OUT_GET_QUOTATION.priceListName = dejsResult_MSIG.vehPrem[i].plan.ToString();
								OUT_GET_QUOTATION.insuranceCompany = "MSIG";
								OUT_GET_QUOTATION.carNo = null;
								OUT_GET_QUOTATION.carBrand = null;
								OUT_GET_QUOTATION.carModel = null;
								OUT_GET_QUOTATION.carEngineCC = dejsResult_MSIG.vehPrem[i].cc.ToString();
								OUT_GET_QUOTATION.carRegisYear = null;

								if (dejsResult_MSIG.vehPrem[i].cc.ToString() == "A001")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";

								}
								else if (dejsResult_MSIG.vehPrem[i].cc.ToString() == "D001")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
								}

								if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "1")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 1";
								}
								else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "2")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2";
								}
								else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "3")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 3";
								}
								else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "2+")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2+";
								}
								else if (dejsResult_MSIG.vehPrem[i].cmp_flag.ToString() == "3+")
								{
									OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 3+";
								}

								OUT_GET_QUOTATION.carFixType = dejsResult_MSIG.vehPrem[i].cmp_flag.ToString();
								/*OUT_GET_QUOTATION.carInsuranceType = dejsResult_MSIG.vehPrem[i].cmp_flag.ToString();*/
								OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_MSIG.vehPrem[i].suminsured.ToString();
								OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_MSIG.vehPrem[i].vmi_prem_gross_amount.ToString();
								OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_stamp_amount.ToString();
								OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_vat_amount.ToString();
								OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_prem_due_amount.ToString();
								OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_MSIG.vehPrem[i].tpbiperperson.ToString();
								OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_MSIG.vehPrem[i].tpbiperevent.ToString();
								OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_MSIG.vehPrem[i].tppd.ToString();
								OUT_GET_QUOTATION.thieftAMT = dejsResult_MSIG.vehPrem[i].tlamt.ToString();
								OUT_GET_QUOTATION.fireAMT = dejsResult_MSIG.vehPrem[i].fiamt.ToString();
								OUT_GET_QUOTATION.floodAMT = dejsResult_MSIG.vehPrem[i].flamt.ToString();
								OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_MSIG.vehPrem[i].txamt.ToString();
								OUT_GET_QUOTATION.personalAccidentAMT = null;
								OUT_GET_QUOTATION.accidentDrive = null;
								OUT_GET_QUOTATION.acidentPassenger = null;
								OUT_GET_QUOTATION.disibilityAMT = null;
								OUT_GET_QUOTATION.disibilityDriver = null;
								OUT_GET_QUOTATION.disibilityPassenger = dejsResult_MSIG.vehPrem[i].ry01compenpassenger.ToString();
								OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_MSIG.vehPrem[i].ry02.ToString();
								OUT_GET_QUOTATION.bailBondAMT = dejsResult_MSIG.vehPrem[i].ry03.ToString();
								OUT_GET_QUOTATION.effectiveDate = dejsResult_MSIG.vehPrem[i].effdate.ToString();
								OUT_GET_QUOTATION.expireDate = dejsResult_MSIG.vehPrem[i].expdate.ToString();
								ResultList_QUOTATION.Add(OUT_GET_QUOTATION);

							}
						}
					}
				}



               

                Result_ALL_QUOTATION.status = "SUCCESS";
                Result_ALL_QUOTATION.message = "SUCCESS";
                Result_ALL_QUOTATION.data = ResultList_QUOTATION;


				// Create a log file
				string fileName = "log.txt";
				string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

				if (File.Exists(logFilePath))
				{
					Console.WriteLine("The file already exists.");
				}
				else
				{
					try
					{
						File.Create(logFilePath);
						Console.WriteLine("File created successfully.");
					}
					catch (Exception ex)
					{
						Console.WriteLine("An error occurred while creating the file: " + ex.Message);
					}
				}

				using (StreamWriter writer = File.AppendText(logFilePath))
				{
					writer.WriteLine(txtRequest);
				}

				return new JavaScriptSerializer().Serialize(Result_ALL_QUOTATION);


            }
            catch (Exception e)
            {
                var Result_error = new M_OUTPUT_JMI();
                Result_error.status = "ERROR";
                Result_error.message = e.Message;
                return new JavaScriptSerializer().Serialize(Result_error);
            }



        }




        //Issue_Policy//
        [WebMethod]
        public string FN_KPI_ISSUE_POLICY(
                                  string broker_code //agent_code
                                , string partner_code
                                , string reference_no

                                , string title_code
                                , string title
                                , string first_name
                                , string last_name
                                , string broker_deptcode
                                , string broker_licenseno

                                , string product_type
                                , string plan_code
                                , string package_type
                                , string sum_insured
                                , string total_premium
                                , string period_type
                                , string effective_date
                                , string expiry_date

                                , string insured1_type
                                , string insured1_id_card_type
                                , string insured1_id_card_number
                                , string insured1_title_code
                                , string insured1_title
                                , string insured1_first_name
                                , string insured1_last_name
                                , string insured1_email
                                , string insured1_mobile_no
                                , string insured1_sex

                                //, string insured1_address_house_number
                                , string insured1_subdistrict_code
                                , string insured1_subdistrict
                                , string insured1_district_code
                                , string insured1_district
                                , string insured1_province_code
                                , string insured1_province
                                , string insured1_postcode

                                , string delivery_type
                                , string delivery_address_type

                                , string brand
                                , string model
                                , string sub_model
                                , string model_year
                                , string car_code
                                , string vehicle_type
                                , string plate_province_code
                                , string plate_province
                                , string plate_number
                                , string chassis_no

            )

        {
            var Culture = new CultureInfo("en-US");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            var Result = new KPI_ISSUEPOLICY_OUTPUT.KPI_Result();


            string Response = string.Empty;

            string Status = string.Empty;

            string strUrlKPI = ConfigurationSettings.AppSettings["strUrlKPI"];

            string strissue_api_key = ConfigurationSettings.AppSettings["strissue_api_key"];
            string strAuthorization = ConfigurationSettings.AppSettings["strAuthorization"];
            string strbroker_code = ConfigurationSettings.AppSettings["strbroker_code"];
            string strissue_url = ConfigurationSettings.AppSettings["strissue_url"];
            string strpartner_code = ConfigurationSettings.AppSettings["strpartner_code"];
            string strbroker_licenseno = ConfigurationSettings.AppSettings["strbroker_licenseno"];
            string strbroker_deptcode = ConfigurationSettings.AppSettings["strbroker_deptcode"];


            var body = "";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient(strissue_url + "/v1/issue/motor");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("x-api-key", strissue_api_key);
                request.AddHeader("Content-Type", "application/json");

                var Arr_InsuredAddress = new KPI_ISSUEPOLICY_INSUREDADDRES
                {
                    house_number = "543/21",//*insured1_address_house_number.ToString(), //"543/21"//*/
                    village = "",
                    soi = "วัดวังหิน 5/1", //"วัดวังหิน 5/1"//
                    moo = "1", // หมู่ที่  "1"//
                    road = "", //ถนน//

                    subdistrict_code = insured1_subdistrict.ToString(), //รหัสตำบล "200701" //
                    subdistrict = insured1_subdistrict.ToString(), //ตำบล "ศรีราชา"//
                    district_code = insured1_district_code.ToString(), //รหัสอำเภอ "2007"//
                    district = insured1_district.ToString(), //อำเภอ "ศรีราชา"//
                    province_code = insured1_province_code.ToString(),// รหัสจังหวัด "20" //

                    province = insured1_province.ToString(), //"ชลบุรี"//
                    postcode = insured1_postcode.ToString() //"20110"//

                };

                //รายละเอียดผู้ประกัน//
                var Arr_insured1_information = new KPI_ISSUEPOLICY_INSURED1INFORMATION
                {
                    type = "P", //Type of insurer. - P = ส่วนบุคคล - O = องค์กร//
                    id_card_type = "R", //Type of identity card - R = บัตรประชาชน - I = บัตรข้าราชการ - C = ทะเบียนการค้า - A = บัตรต่างด้าว - P = passport - D = ใบขับขี่ - G = หน่วนงานราชการรับรอง//
                    id_card_number = insured1_id_card_number.ToString(), //card_number 1209700022521//
                    title_code = insured1_title_code.ToString(), //รหัส title "1"//
                    title = insured1_title.ToString(), //คำนำหน้า "นาย"//
                    first_name = insured1_first_name.ToString(), //ชื่อ "ทองดี"//
                    last_name = insured1_last_name.ToString(), //นามสกุล  "มีชัย"//
                    birthdate = "19860115", //วันเกิด YYYYMMD "19860115"//
                    email = insured1_email.ToString(), //email "narutep.c@kpi.co.th"//
                    mobile_no = insured1_mobile_no.ToString(), //เบอร์โทร "0935041965"//
                    nationality = "thai", //สัญชาติ "thai"//
                    social_id = "", ////
                    sex = insured1_sex.ToString(), //Gender - M = male - F = female//
                    marital_status = "S", //Marital Status - S = single - M = married - D = Divorce - U = Undefined/
                    occupation_code = "1013", //รหัสอาชีพ //

                    insured_address = Arr_InsuredAddress

                };

                //ผู้รับผลประโยชน์//
                var Arr_Beneficiary = new KPI_ISSUEPOLICY_BENEFICIARY
                {
                    sequence = "1", //ลำดับ//
                    type = "P", //Type of insurer. - P = ส่วนบุคคล - O = องค์กร//
                    title_code = "", //รหัสคำนำหน้า//
                    title = "", //คำนำหน้า//
                    first_name = "ทายาทตามกฎหมาย",
                    last_name = "",
                    relation_code = "", //รหัสความสัมพันธ์//
                    relation = "ทายาทตามกฎหมาย", //ความสัมพันธ์//
                    percent = "100" //Percent การได้รับผลประโยชน์//

                };

                //รายละเอียดของรถ//
                var Arr_VehicleInformation = new KPI_ISSUEPOLICY_VEHICLEINFORMATION
                {
                    brand = brand.ToString(), //"Mazda",
                    model = model.ToString(), //"CX-3",
                    sub_model = sub_model.ToString(), //"Wagon 4dr Base Plus SA 6sp Front Wheel Drive 2.0i",
                    model_year = model_year.ToString(), //"2021",
                    car_code = "1.10", //"1.10",
                    vehicle_type = vehicle_type.ToString(), //"110",
                    plate_province_code = plate_province_code.ToString(), //"10",
                    plate_province = "กรุงเทพมหานคร", //"กรุงเทพมหานคร",
                    plate_number = plate_number.ToString(), //"บง7372",
                    engine_capacity = "1998",//"1998",
                    driver_passenger = 7,
                    color = "",
                    chassis_no = chassis_no.ToString(), //"MRHFD1654AP103711",
                    engine_no = ""

                };

                // รายละเอียด โบรกเกอร์ //
                var Arr_BrokerInformation = new KPI_ISSUEPOLICY_BROKERINFORMATION
                {

                    broker_code = strbroker_code.ToString(), //"0032003939",//
                    title_code = "",
                    title = "บริษัท",
                    first_name = "ซิงเกอร์",
                    last_name = "จำกัด",
                    broker_deptcode = strbroker_deptcode.ToString(), //"0032003939",//
                    broker_licenseno = strbroker_licenseno.ToString() //"วว-9999999"//

                };

                //ที่อยู่จัดส่ง//
                var Arr_DeliveryAddress = new KPI_ISSUEPOLICY_DELIVERYADDRESS
                {
                    house_number = "543/21",
                    village = "",
                    soi = "วัดวังหิน 5/1",
                    moo = "1",
                    road = "",

                    subdistrict_code = insured1_subdistrict.ToString(), //"200701",                         
                    subdistrict = insured1_subdistrict.ToString(), //"ศรีราชา",
                    district_code = insured1_district_code.ToString(), //"2007",
                    district = insured1_district.ToString(), //"ศรีราชา",
                    province_code = insured1_province_code.ToString(), //"20",
                    province = insured1_province.ToString(), //"ชลบุรี",
                    postcode = insured1_postcode.ToString() //"20110"


                    //subdistrict_code = "200701",
                    //subdistrict = "ศรีราชา",
                    //district_code = "2007",ุ
                    //district = "ศรีราชา",
                    //province_code = "20",       
                    //province = "ชลบุรี",
                    //postcode = "20110"

                };

                //รายละเอียดการชำระ//
                var Arr_purchase_information = new KPI_ISSUEPOLICY_PURCHASEINFORMATION
                {
                    title_code = "",
                    title = "",
                    first_name = "",
                    last_name = "",
                    card_type = "",
                    card_number = ""

                };

                //ที่อยู่ออกใบเสร็จ//
                var Arr_receipt_information = new KPI_ISSUEPOLICY_RECEIPTINFORMATION
                {
                    type = "",
                    card_type = "",
                    card_number = "",
                    title_code = "",
                    title = "",
                    first_name = "",
                    last_name = "",
                    house_number = "543/21",
                    village = "",
                    soi = "วัดวังหิน 5/1",
                    moo = "1",
                    road = "",

                    subdistrict_code = insured1_subdistrict.ToString(), //"200701",                         
                    subdistrict = insured1_subdistrict.ToString(), //"ศรีราชา",
                    district_code = insured1_district_code.ToString(), //"2007",
                    district = insured1_district.ToString(), //"ศรีราชา",
                    province_code = insured1_province_code.ToString(), //"20",
                    province = insured1_province.ToString(), //"ชลบุรี",
                    postcode = insured1_postcode.ToString() //"20110"

                };

                //รายละเอียด Payment//
                var Arr_PaymentInformation = new KPI_ISSUEPOLICY_PAYMENTINFORMATION
                {
                    payee_type = "I",
                    type = "",
                    card_type = "",
                    card_number = "",
                    payment_mode = "CA",
                    title_code = "",
                    title = "",
                    first_name = "",
                    last_name = "",
                    payment_status = "",
                    payment_authapprovecode = "",
                    bank_code = "",
                    bookbank_number = "",
                    creditcard_type = "",
                    creditcard_number = ""
                };



                var Arr_Body = new
                {
                    agent_code = strbroker_code, //032003939//
                    master_code = strbroker_code, //032003939//
                    partner_code = strpartner_code, //Sawasdee//
                    reference_no = "0032003939-230125-0000002", //0032003939-230125-0000002//
                    product_type = product_type.ToString(), //vmi+cmi//
                    plan_code = plan_code.ToString(), //SGB31100//
                    package_type = package_type.ToString(), //1,2,3,2+,3+//
                    mc_sticker = "",
                    sum_insured = Int32.Parse(sum_insured.ToString()), //0//
                    total_premium = double.Parse(total_premium.ToString()), // 2199.92// 
                    period_type = "Y",
                    effective_date = effective_date.ToString(), //2023-04-19//
                    expiry_date = expiry_date.ToString(), //2023-04-19//

                    insured1_information = Arr_insured1_information, //insured1_information & InsuredAddress//

                    beneficiary = new[] { Arr_Beneficiary }, //array//


                    delivery_type = delivery_type.ToString(), //E,P//
                    delivery_address_type = delivery_address_type.ToString(), //I = ที่อยู่เดียวกับ insured address O = อื่นๆโปรดระบุ//

                    delivery_address = Arr_DeliveryAddress,

                    payment_information = Arr_PaymentInformation,

                    purchase_information = Arr_purchase_information,

                    receipt_information = Arr_receipt_information,

                    vehicle_information = Arr_VehicleInformation,

                    broker_information = Arr_BrokerInformation

                };


                body = JsonConvert.SerializeObject(Arr_Body);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                Status = "SUCCESS";
                Response = response.Content;
                Result = serializer.Deserialize<KPI_ISSUEPOLICY_OUTPUT.KPI_Result>(response.Content);

            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG(body, Status, "KPI");
            }
            finally
            {

            }

            return new JavaScriptSerializer().Serialize(Result);

        }

        [WebMethod]

        public string FN_KPI_GET_COMMON_SERVICE()
        {
            var Culture = new CultureInfo("en-US");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            var Result = new KPI_GET_COMMON_SERVICE_DATA_PROVINCE_ROOT();


            string Response = string.Empty;

            string Status = string.Empty;

            string strUrlKPI = ConfigurationSettings.AppSettings["strUrlKPI"];

            string strissue_api_key = ConfigurationSettings.AppSettings["strissue_api_key"];
            string strAuthorization = ConfigurationSettings.AppSettings["strAuthorization"];
            string strbroker_code = ConfigurationSettings.AppSettings["strbroker_code"];
            string strissue_url = ConfigurationSettings.AppSettings["strissue_url"];


            var body = "";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient(strissue_url + "/v1/master_data/provinces");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                request.AddHeader("x-api-key", "AIzaSyADXlVVR6vXKaYJ2JeZzjgGn9Sh7fWQSDk");

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                Status = "success";
                Response = response.Content;
                //Result = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(response.Content);
                Result = serializer.Deserialize<KPI_GET_COMMON_SERVICE_DATA_PROVINCE_ROOT>(response.Content);


            }
            catch (Exception Error)
            {

                Status = Error.Message.ToString();
                FN_KEEP_LOG(body, Status, "KPI");
            }
            finally
            {



            }

            return new JavaScriptSerializer().Serialize(Result);


        }



		/*[WebMethod]
		public string FN_SEND_INSURANCE(string InsuranceCompany, string carBrand, string carModel, string carYear, string carRegisYear, string carNo, string carProvince, string carEngineCC, string carWeight, string carCamera, string carFixType, string carInsuranceType, string identifyDrivers, string driverName1, string driverName2, string driverBirthDate1, string driverBirthDate2, string Inception_date, string Expiry_date, string Number_Of_Seating)
		{



			string txtRequest = "< carBrand > " + carBrand + " </ carBrand >" +
								  "< carModel >  " + carModel + " </ carModel >" +
								  "< carYear >  " + carYear + " </ carYear >" +
								  "< carRegisYear >  " + carRegisYear + " </ carRegisYear >" +
								  "< InsuranceCompany >  " + InsuranceCompany + " </ InsuranceCompany >" +
								  "< carNo >  " + carNo + " </ carNo >" +
								  "< carProvince >  " + carProvince + " </ carProvince >" +
								  "< carEngineCC >  " + carEngineCC + " </ carEngineCC >" +
								  "< carWeight >  " + carWeight + " </ carWeight >" +
								  "< carCamera >  " + carCamera + " </ carCamera >" +
								  "< carFixType >  " + carFixType + " </ carFixType >" +
								  "< carInsuranceType >  " + carInsuranceType + " </ carInsuranceType >" +
								  "< identifyDrivers >  " + identifyDrivers + " </ identifyDrivers >" +
								  "< driverName1 > " + driverName1 + "</ driverName1 >" +
								  "< driverName2 > " + driverName2 + "</ driverName2 >" +
								  "< driverBirthDate1 > " + driverBirthDate1 + "</ driverBirthDate1 >" +
								  "< driverBirthDate2 > " + driverBirthDate2 + "</ driverBirthDate2 >" +
								  "< Inception_date >  " + Inception_date + " </ Inception_date >" +
								  "< Expiry_date >  " + Expiry_date + " </ Expiry_date >" +
								  "< Number_Of_Seating >  " + Number_Of_Seating + " </ Number_Of_Seating >";


			if (Inception_date.Trim() == "" || Inception_date.Trim() == "null")
			{
				Inception_date = "2022-03-01";
			}
			if (Expiry_date.Trim() == "" || Expiry_date.Trim() == "null")
			{
				Expiry_date = "2023-03-01";
			}

			carBrand = char.ToUpper(carBrand[0]) + carBrand.Substring(1).ToLower();
			carModel = char.ToUpper(carModel[0]) + carModel.Substring(1).ToLower();

			string MSIG_IdentifyDriver = string.Empty;
			string FLAG_CCTV = string.Empty;
			var dejsResult_VIB = new VIB_GET_QUOTATION();
			var dejsResult_JMI = new M_OUTPUT_JMI();
			var dejsResult_MSIG = new M_OUTPUT_MSIG();

			//bendcemb เริ่ม//
			var dejsResult_KPI = new KPI_GET_QUOTATION.PlanList();

			var Result_ALL_QUOTATION = new M_OUTPUT_ALL_INSURANCE();

			string list_driversList = string.Empty;
			if (driverBirthDate1 != "" && driverBirthDate2 != "")
			{

				list_driversList = driverBirthDate1 + "," + driverBirthDate2;

			}
			else if (driverBirthDate1 != "")
			{

				list_driversList = driverBirthDate1;

			}
			else if (driverBirthDate2 != "")
			{

				list_driversList = driverBirthDate2;
			}

			JavaScriptSerializer serializer = new JavaScriptSerializer();

			var ResultList_QUOTATION = new List<M_OUTPUT_ALL_INSURANCE_DATA>();

			try
			{

				string input = InsuranceCompany.ToString();
				char[] delimiter = { ',' };

				string[] InsuranceCompanyList = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

				string VIB = "VIB";
				bool VIBExists = Array.IndexOf(InsuranceCompanyList, VIB) != -1;
				if (VIBExists)
				{
					var Result_VIB_GET_QUOTATION = FN_VIB_GET_QUOTATION(" ", carBrand.ToUpper(), carModel.ToUpper(), carRegisYear,carNo, "", "", list_driversList, "", carInsuranceType, carFixType);
					dejsResult_VIB = serializer.Deserialize<VIB_GET_QUOTATION>(Result_VIB_GET_QUOTATION);
					if (dejsResult_VIB.integrationStatusCode == "0")
					{
						for (int i = 0; i < dejsResult_VIB.data.Count(); i++)
						{
							var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();


							OUT_GET_QUOTATION.priceListCode = dejsResult_VIB.data[i].packageCode.ToString();
							OUT_GET_QUOTATION.priceListName = dejsResult_VIB.data[i].packageName.ToString();
							OUT_GET_QUOTATION.insuranceCompany = "VIB";
							OUT_GET_QUOTATION.carNo = dejsResult_VIB.data[i].vehicleTypeCode.ToString();
							OUT_GET_QUOTATION.carBrand = dejsResult_VIB.data[i].carBrand.ToString();
							OUT_GET_QUOTATION.carModel = dejsResult_VIB.data[i].carModel.ToString();
							OUT_GET_QUOTATION.carEngineCC = dejsResult_VIB.data[i].engineCC.ToString();
							OUT_GET_QUOTATION.carRegisYear = dejsResult_VIB.data[i].registrationYear.ToString();

							if (dejsResult_VIB.data[i].repairType.ToString() == "G")
							{
								OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";

							}
							else if (dejsResult_VIB.data[i].repairType.ToString() == "D")
							{
								OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
							}


							if (dejsResult_VIB.data[i].insuranceType.ToString() == "1")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 1";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "2")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 2";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "3")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 3";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "4")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท 4";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "2P")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 2+";
							}
							else if (dejsResult_VIB.data[i].insuranceType.ToString() == "3P")
							{
								OUT_GET_QUOTATION.carInsuranceType = "ประเภท5 3+";
							}

							//OUT_GET_QUOTATION.carFixType = dejsResult_VIB.data[i].repairType.ToString();

							//OUT_GET_QUOTATION.carInsuranceType = dejsResult_VIB.data[i].insuranceType.ToString();
							OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_VIB.data[i].ownDamage.sumInsured.ToString();
							OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_VIB.data[i].netPremium.ToString();
							OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_VIB.data[i].stamp.ToString();
							OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_VIB.data[i].vat.ToString();
							OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_VIB.data[i].totalPremium.ToString();
							OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_VIB.data[i].liability.tpbiPerPerson.ToString();
							OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_VIB.data[i].liability.tpbiPerEvent.ToString();
							OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_VIB.data[i].liability.tppdPerEvent.ToString();
							OUT_GET_QUOTATION.thieftAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
							OUT_GET_QUOTATION.fireAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
							OUT_GET_QUOTATION.floodAMT = dejsResult_VIB.data[i].ownDamage.tfSumInsured.ToString();
							OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_VIB.data[i].ownDamage.deductible.ToString();
							if (dejsResult_VIB.data[i].additionalCoverage.Count != 0)
							{
								OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_VIB.data[i].additionalCoverag[0].personAccident.driverDeath.ToString();
								OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_VIB.data[i].additionalCoverag[0].personAccident.driverDeath.ToString();
								OUT_GET_QUOTATION.accidentDrive = null;
								OUT_GET_QUOTATION.acidentPassenger = dejsResult_VIB.data[i].additionalCoverag[0].personAccident.numberPassengerTemporaryDisability.ToString();
								OUT_GET_QUOTATION.disibilityAMT = dejsResult_VIB.data[i].additionalCoverag[0].personAccident.temporaryDisabilityPassenger.ToString();
								OUT_GET_QUOTATION.disibilityDriver = null;
								OUT_GET_QUOTATION.disibilityPassenger = dejsResult_VIB.data[i].additionalCoverag[0].personAccident.numberPassengerTemporaryDisability.ToString();
								OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_VIB.data[i].additionalCoverag[0].medicalExpense.ToString();
								OUT_GET_QUOTATION.bailBondAMT = dejsResult_VIB.data[i].additionalCoverage[0].bailBond.ToString();
							}
							else
							{
								OUT_GET_QUOTATION.personalAccidentAMT = null;
								OUT_GET_QUOTATION.personalAccidentAMT = null;
								OUT_GET_QUOTATION.accidentDrive = null;
								OUT_GET_QUOTATION.acidentPassenger = null;
								OUT_GET_QUOTATION.disibilityAMT = null;
								OUT_GET_QUOTATION.disibilityDriver = null;
								OUT_GET_QUOTATION.disibilityPassenger = null;
								OUT_GET_QUOTATION.medicalExpenseAMT = null;
								OUT_GET_QUOTATION.bailBondAMT = null;
							}

							OUT_GET_QUOTATION.effectiveDate = null;
							OUT_GET_QUOTATION.expireDate = null;
							ResultList_QUOTATION.Add(OUT_GET_QUOTATION);

						}

					}
				}

				string JMI = "JMI";
				bool JMIExists = Array.IndexOf(InsuranceCompanyList, JMI) != -1;
				if (JMIExists)
				{
					var Result_JMI_GET_QUOTATION = FN_JMI_GET_QUOTATION("MOTOR_PACKAGE_SGB", carBrand, carModel, carYear,carEngineCC, carNo);
					dejsResult_JMI = serializer.Deserialize<M_OUTPUT_JMI>(Result_JMI_GET_QUOTATION);
					if (dejsResult_JMI.status == "200")
					{

						for (int i = 0; i < dejsResult_JMI.data.Count; i++)
						{
							var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();
							OUT_GET_QUOTATION.priceListCode = dejsResult_JMI.data[i].Package_code.ToString();
							OUT_GET_QUOTATION.priceListName = dejsResult_JMI.data[i].Package_name.ToString();
							OUT_GET_QUOTATION.insuranceCompany = "JMI";
							OUT_GET_QUOTATION.carNo = null;
							OUT_GET_QUOTATION.carBrand = null;
							OUT_GET_QUOTATION.carModel = null;
							OUT_GET_QUOTATION.carEngineCC = null;
							OUT_GET_QUOTATION.carRegisYear = null;
							OUT_GET_QUOTATION.carFixType = dejsResult_JMI.data[i].Type_garage.ToString();
							OUT_GET_QUOTATION.carInsuranceType = dejsResult_JMI.data[i].Insurance_type.ToString();

							OUT_GET_QUOTATION.carInsuranceTypeName = dejsResult_JMI.data[i].Insurance_type.ToString();

							OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_JMI.data[i].Sum_insure.ToString();
							OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_JMI.data[i].Premium.ToString();
							OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_JMI.data[i].Duty.ToString();
							OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_JMI.data[i].Vat.ToString();
							OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_JMI.data[i].Total_insurance.ToString();
							OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_JMI.data[i].Damage_life_per_person.ToString();
							OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_JMI.data[i].Damage_life_per_time.ToString();
							OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_JMI.data[i].Damage_property.ToString();
							OUT_GET_QUOTATION.thieftAMT = dejsResult_JMI.data[i].Damage_lost.ToString();
							OUT_GET_QUOTATION.fireAMT = dejsResult_JMI.data[i].Damage_fire.ToString();
							OUT_GET_QUOTATION.floodAMT = dejsResult_JMI.data[i].Damage_flood.ToString();
							OUT_GET_QUOTATION.carDecitibleAMT = null;
							OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_JMI.data[i].Damage_death_permanent_disability.ToStrin();
							OUT_GET_QUOTATION.accidentDrive = dejsResult_JMI.data[i].Coverage_amount_driver.ToString();
							OUT_GET_QUOTATION.acidentPassenger = dejsResult_JMI.data[i].Coverage_amount_passengers.ToString();
							OUT_GET_QUOTATION.disibilityAMT = dejsResult_JMI.data[i].Damage_temporary_disability.ToString();
							OUT_GET_QUOTATION.disibilityDriver = dejsResult_JMI.data[i].Coverage_amount_driver.ToString();
							OUT_GET_QUOTATION.disibilityPassenger = dejsResult_JMI.data[i].Coverage_amount_passengers.ToString();
							OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_JMI.data[i].Medical_expenses.ToString();
							OUT_GET_QUOTATION.bailBondAMT = dejsResult_JMI.data[i].Driver_bail.ToString();
							OUT_GET_QUOTATION.effectiveDate = null;
							OUT_GET_QUOTATION.expireDate = null;
							ResultList_QUOTATION.Add(OUT_GET_QUOTATION);


						}
					}
				}

				string KPI = "KPI";
				bool KPIExists = Array.IndexOf(InsuranceCompanyList, KPI) != -1;
				if (KPIExists)
				{
					var Result_KPI_GET_QUOTATION = FN_KPI_GET_QUOTATION("0032003939", carBrand, carModel, carYear, carInsuranceType);
					dejsResult_KPI = serializer.Deserialize<KPI_GET_QUOTATION.PlanList>(Result_KPI_GET_QUOTATION);
					if (dejsResult_KPI.result.resultMessage == "success")
					{

						for (int i = 0; i < dejsResult_KPI.plan.Count; i++)
						{
							var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();

							OUT_GET_QUOTATION.priceListName = dejsResult_KPI.plan[i].planNameTh.ToString();
							OUT_GET_QUOTATION.insuranceCompany = "KPI";

							//planlist//
							if (dejsResult_KPI.plan[i].plan.Count != 0)
							{
								OUT_GET_QUOTATION.priceListCode = dejsResult_KPI.plan[i].si.ToString();

								OUT_GET_QUOTATION.carNo = dejsResult_KPI.plan[i].plan[0].vehicleCode.ToString();

								OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
								OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_KPI.plan[i].plan[0].netPrem.ToString();
								OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_KPI.plan[i].plan[0].stamp.ToString();
								OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_KPI.plan[i].plan[0].vat.ToString();
								OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_KPI.plan[i].plan[0].totalPrem.ToString();

								OUT_GET_QUOTATION.effectiveDate = dejsResult_KPI.plan[i].plan[0].effectiveDate.ToString();
								OUT_GET_QUOTATION.expireDate = dejsResult_KPI.plan[i].plan[0].expiryDate.ToString();

								OUT_GET_QUOTATION.thieftAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
								OUT_GET_QUOTATION.fireAMT = dejsResult_KPI.plan[i].plan[0].sumInsured.ToString();
								OUT_GET_QUOTATION.floodAMT = null;
							}
							else
							{
								OUT_GET_QUOTATION.priceListCode = null;
								OUT_GET_QUOTATION.carNo = null;

								OUT_GET_QUOTATION.personalAccidentAMT = null;
								OUT_GET_QUOTATION.premiumInsuranceAMT = null;
								OUT_GET_QUOTATION.stampInsuranceTotal = null;
								OUT_GET_QUOTATION.vatInsuranceTotal = null;
								OUT_GET_QUOTATION.premiumInsuranceTotal = null;

								OUT_GET_QUOTATION.effectiveDate = null;
								OUT_GET_QUOTATION.expireDate = null;

								OUT_GET_QUOTATION.thieftAMT = null;
								OUT_GET_QUOTATION.fireAMT = null;
							}


							OUT_GET_QUOTATION.carBrand = dejsResult_KPI.plan[i].brand.ToString();
							OUT_GET_QUOTATION.carModel = dejsResult_KPI.plan[i].model.ToString();
							OUT_GET_QUOTATION.carEngineCC = dejsResult_KPI.plan[i].engineCapacity.ToString();
							OUT_GET_QUOTATION.carRegisYear = dejsResult_KPI.plan[i].carManufactureYear.ToString();


							if (dejsResult_KPI.plan[i].repairOption.ToString() == "G")
							{
								OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";
							}
							else if (dejsResult_KPI.plan[i].repairOption.ToString() == "D")
							{
								OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
							}


							//OUT_GET_QUOTATION.carFixType = dejsResult_KPI.plan[i].repairOption.ToString();

							OUT_GET_QUOTATION.carInsuranceType = dejsResult_KPI.plan[i].motorClass.ToString();

							//CoverageList
							if (dejsResult_KPI.plan[i].coverageList.Count != 0)
							{
								if (dejsResult_KPI.plan[i].motorClass.ToString() == "1")
								{
									OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_KPI.plan[i].coverageList[3].coverageAmt.ToString();
									OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_KPI.plan[i].coverageList[4].coverageAmt.ToStrin();
									OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_KPI.plan[i].coverageLis[5].coverageAmt.ToString();

								}
								else
								{
									OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_KPI.plan[i].coverageList[4].coverageAmt.ToString();
									OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_KPI.plan[i].coverageList[5].coverageAmt.ToStrin();
									OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_KPI.plan[i].coverageLis[6].coverageAmt.ToString();
								}

								OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_KPI.plan[i].coverageList[1].coverageAmt.ToString();
								OUT_GET_QUOTATION.personalAccidentAMT = dejsResult_KPI.plan[i].coverageList[7].coverageAmt.ToStrin();
								OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_KPI.plan[i].coverageList[8].coverageAmt.ToString();
								OUT_GET_QUOTATION.bailBondAMT = dejsResult_KPI.plan[i].coverageList[9].coverageAmt.ToString();
							}
							else
							{
								OUT_GET_QUOTATION.bodyPersonAMT = null;
								OUT_GET_QUOTATION.accidentPersonAMT = null;
								OUT_GET_QUOTATION.propertiesPersonAMT = null;


								OUT_GET_QUOTATION.carDecitibleAMT = null;
								OUT_GET_QUOTATION.personalAccidentAMT = null;

								OUT_GET_QUOTATION.medicalExpenseAMT = null;
								OUT_GET_QUOTATION.bailBondAMT = null;
							}



							OUT_GET_QUOTATION.floodAMT = null;
							OUT_GET_QUOTATION.accidentDrive = null;
							OUT_GET_QUOTATION.acidentPassenger = null;
							OUT_GET_QUOTATION.disibilityAMT = null;
							OUT_GET_QUOTATION.disibilityDriver = null;
							OUT_GET_QUOTATION.disibilityPassenger = null;

							ResultList_QUOTATION.Add(OUT_GET_QUOTATION);


						}
					}
				}

				string MSIG = "MSIG";
				bool MSIGExists = Array.IndexOf(InsuranceCompanyList, MSIG) != -1;
				if (MSIGExists)
					{
						*//*DateTime DfInception_date = Convert.ToDateTime(Inception_date);*//*

						char[] delimiters = { '-' };
						string[] DfInception_date = Inception_date.Split(delimiters, StringSplitOptions.None);
						string SR_Inception_date = DfInception_date[0].ToString() + DfInception_date[1].ToString() + DfInception_date[2].ToString();

						*//*DateTime DfExpiry_date = Convert.ToDateTime(Expiry_date);*//*
						string[] DfExpiry_date = Expiry_date.Split(delimiters, StringSplitOptions.None);
						string SR_Expiry_date = DfExpiry_date[0].ToString() + DfExpiry_date[1].ToString() + DfExpiry_date[2].ToString();

						if (identifyDrivers.ToLower() == "true")
						{

							var length_listdrivers = list_driversList.Split(',');
							MSIG_IdentifyDriver = length_listdrivers.Length.ToString();


						}
						else if (identifyDrivers.ToLower() == "false")
						{

							MSIG_IdentifyDriver = "0";

						}
						if (carCamera.ToLower() == "true")
						{

							FLAG_CCTV = "Y";

						}
						else if (carCamera.ToLower() == "false")
						{
							FLAG_CCTV = "N";

						}


						*//*var Result_MSIG_GET_QUOTATION = FN_MSIG_GET_QUOTATION("R", "Honda", "Civic", "", "", "20220301", "20230301", "0", "N", "110", "", "", "WS-SGB", "2021", 0, 0, "01", "BCA8292", "P", "N", "", 0, "7", "1500", 0);*//*

						var Result_MSIG_GET_QUOTATION = FN_MSIG_GET_QUOTATION("R", carBrand, carModel, "", "", SR_Inception_date, SR_Expiry_date, MSIG_IdentifyDriver, "N", carNo.ToString(), "", "", "WS-SGB", carRegisYear, 0, 0, "01", "BCA8292", "P", FLAG_CCTV, "", 0, Number_Of_Seating, carEngineCC, 0);
						dejsResult_MSIG = serializer.Deserialize<M_OUTPUT_MSIG>(Result_MSIG_GET_QUOTATION);
						if (dejsResult_MSIG.success.ToLower() == "true")
						{
							for (int i = 0; i < dejsResult_MSIG.vehPrem.Count; i++)
							{
								var OUT_GET_QUOTATION = new M_OUTPUT_ALL_INSURANCE_DATA();
								OUT_GET_QUOTATION.priceListCode = dejsResult_MSIG.vehPrem[i].packagecode.ToString();
								OUT_GET_QUOTATION.priceListName = dejsResult_MSIG.vehPrem[i].plan.ToString();
								OUT_GET_QUOTATION.insuranceCompany = "MSIG";
								OUT_GET_QUOTATION.carNo = null;
								OUT_GET_QUOTATION.carBrand = null;
								OUT_GET_QUOTATION.carModel = null;
								OUT_GET_QUOTATION.carEngineCC = dejsResult_MSIG.vehPrem[i].cc.ToString();
								OUT_GET_QUOTATION.carRegisYear = null;

								if (dejsResult_MSIG.vehPrem[i].cc.ToString() == "A001")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมอู่";

								}
								else if (dejsResult_MSIG.vehPrem[i].cc.ToString() == "D001")
								{
									OUT_GET_QUOTATION.carFixType = "ซ่อมห้าง";
								}
								OUT_GET_QUOTATION.carFixType = dejsResult_MSIG.vehPrem[i].cmp_flag.ToString();
								OUT_GET_QUOTATION.carInsuranceType = dejsResult_MSIG.vehPrem[i].cmp_flag.ToString();
								OUT_GET_QUOTATION.sumInsuranceAMT = dejsResult_MSIG.vehPrem[i].suminsured.ToString();
								OUT_GET_QUOTATION.premiumInsuranceAMT = dejsResult_MSIG.vehPrem[i].vmi_prem_gross_amount.ToString();
								OUT_GET_QUOTATION.stampInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_stamp_amount.ToString();
								OUT_GET_QUOTATION.vatInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_vat_amount.ToString();
								OUT_GET_QUOTATION.premiumInsuranceTotal = dejsResult_MSIG.vehPrem[i].vmi_prem_due_amount.ToString();
								OUT_GET_QUOTATION.bodyPersonAMT = dejsResult_MSIG.vehPrem[i].tpbiperperson.ToString();
								OUT_GET_QUOTATION.accidentPersonAMT = dejsResult_MSIG.vehPrem[i].tpbiperevent.ToString();
								OUT_GET_QUOTATION.propertiesPersonAMT = dejsResult_MSIG.vehPrem[i].tppd.ToString();
								OUT_GET_QUOTATION.thieftAMT = dejsResult_MSIG.vehPrem[i].tlamt.ToString();
								OUT_GET_QUOTATION.fireAMT = dejsResult_MSIG.vehPrem[i].fiamt.ToString();
								OUT_GET_QUOTATION.floodAMT = dejsResult_MSIG.vehPrem[i].flamt.ToString();
								OUT_GET_QUOTATION.carDecitibleAMT = dejsResult_MSIG.vehPrem[i].txamt.ToString();
								OUT_GET_QUOTATION.personalAccidentAMT = null;
								OUT_GET_QUOTATION.accidentDrive = null;
								OUT_GET_QUOTATION.acidentPassenger = null;
								OUT_GET_QUOTATION.disibilityAMT = null;
								OUT_GET_QUOTATION.disibilityDriver = null;
								OUT_GET_QUOTATION.disibilityPassenger = dejsResult_MSIG.vehPrem[i].ry01compenpassenger.ToString();
								OUT_GET_QUOTATION.medicalExpenseAMT = dejsResult_MSIG.vehPrem[i].ry02.ToString();
								OUT_GET_QUOTATION.bailBondAMT = dejsResult_MSIG.vehPrem[i].ry03.ToString();
								OUT_GET_QUOTATION.effectiveDate = dejsResult_MSIG.vehPrem[i].effdate.ToString();
								OUT_GET_QUOTATION.expireDate = dejsResult_MSIG.vehPrem[i].expdate.ToString();
								ResultList_QUOTATION.Add(OUT_GET_QUOTATION);

							}
						}
					}
				


				Result_ALL_QUOTATION.status = "SUCCESS";
				Result_ALL_QUOTATION.message = "SUCCESS";
				Result_ALL_QUOTATION.data = ResultList_QUOTATION;


				// Create a log file
				string fileName = "log_insurance.txt";
				string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

				if (File.Exists(logFilePath))
				{
					Console.WriteLine("The file already exists.");
				}
				else
				{
					try
					{
						File.Create(logFilePath);
						Console.WriteLine("File created successfully.");
					}
					catch (Exception ex)
					{
						Console.WriteLine("An error occurred while creating the file: " + ex.Message);
					}
				}

				using (StreamWriter writer = File.AppendText(logFilePath))
				{
					writer.WriteLine(txtRequest);
				}

				return new JavaScriptSerializer().Serialize(Result_ALL_QUOTATION);


			}
			catch (Exception e)
			{
				var Result_error = new M_OUTPUT_JMI();
				Result_error.status = "ERROR";
				Result_error.message = e.Message;
				return new JavaScriptSerializer().Serialize(Result_error);
			}



		}*/

	}
}
