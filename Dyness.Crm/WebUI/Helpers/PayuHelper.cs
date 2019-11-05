using Core.General;
using Newtonsoft.Json;
using Payu.Core;
using Payu.Core.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace WebUI.Helpers
{
    public class PayuHelper
    {
        public List<KeyValuePair<string, string>> ReturnCodes = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("AUTHORIZED", "Başarılı yetkili. (Hata Kodu:AUTHORIZED)"),
            new KeyValuePair<string, string>("3DS_ENROLLED", "3D kaydedilmiş. (Hata Kodu:3DS_ENROLLED)"),
            new KeyValuePair<string, string>("ALREADY_AUTHORIZED","Sipariş zaten ödeme belirleniş. (Hata Kodu:ALREADY_AUTHORIZED)"),
            new KeyValuePair<string, string>("AUTHORIZATION_FAILED", "Yetki başarısız. (Hata Kodu:AUTHORIZATION_FAILED)"),
            new KeyValuePair<string, string>("INVALID_CUSTOMER_INFO","Geçersiz müşteri bilgileri. (Hata Kodu:INVALID_CUSTOMER_INFO)"),
            new KeyValuePair<string, string>("INVALID_PAYMENT_INFO","Geçersiz ödeme bilgileri. (Hata Kodu:INVALID_PAYMENT_INFO)"),
            new KeyValuePair<string, string>("INVALID_ACCOUNT", "Geçersiz hesap. (Hata Kodu:INVALID_ACCOUNT)"),
            new KeyValuePair<string, string>("INVALID_PAYMENT_METHOD_CODE","Geçersiz ödeme methodu. (Hata Kodu:INVALID_PAYMENT_METHOD_CODE)"),
            new KeyValuePair<string, string>("INVALID_CURRENCY", "Geçersiz para birimi. (Hata Kodu:INVALID_CURRENCY)"),
            new KeyValuePair<string, string>("REQUEST_EXPIRED", "İsteğin süresi dolmuş. (Hata Kodu:REQUEST_EXPIRED)"),
            new KeyValuePair<string, string>("HASH_MISMATCH", "Hash Uyumsuzluğu. (Hata Kodu:HASH_MISMATCH)"),
            new KeyValuePair<string, string>("WRONG_VERSION", "Yanlış versiyon. (Hata Kodu:WRONG_VERSION)"),
            new KeyValuePair<string, string>("INVALID_CC_TOKEN", "Geçersiz CC. (Hata Kodu:INVALID_CC_TOKEN)"),
            new KeyValuePair<string, string>("INSTALLMENTS_LOYALTY_POINTS_INCOMPATIBLE","Taksitli satış uyumsuzluğu.(Hata Kodu:INSTALLMENTS_LOYALTY_POINTS_INCOMPATIBLE)"),
            new KeyValuePair<string, string>("GW_ERROR_GENERIC","İşlem gerçekleşirken bir hata oluştu. Lütfen tekrar deneyin. (Hata Kodu:GW_ERROR_GENERIC)"),
            new KeyValuePair<string, string>("GW_ERROR_GENERIC_3D ","3D Secure işlemi sırasında bir hata oluştu. (Hata Kodu:GW_ERROR_GENERIC_3D)"),
            new KeyValuePair<string, string>("GWERROR_-19", "Kimlik doğrulama başarısız oldu. (Hata Kodu:GWERROR_-19)"),
            new KeyValuePair<string, string>("GWERROR_-9","Kart son kullanma tarihi alanında hata. (Hata Kodu:GWERROR_-9)"),
            new KeyValuePair<string, string>("GWERROR_-8", "Geçersiz kart numarası. (Hata Kodu:GWERROR_-8)"),
            new KeyValuePair<string, string>("GWERROR_-3", "Çağrı merkezini arayın. (Hata Kodu:GWERROR_-3)"),
            new KeyValuePair<string, string>("GWERROR_-2","İşlem gerçekleşirken bir hata oluştu. Lütfen tekrar deneyin. (Hata Kodu:GWERROR_-2)"),
            new KeyValuePair<string, string>("GWERROR_03", "Geçersiz firma. (Hata Kodu:GWERROR_03)"),
            new KeyValuePair<string, string>("GWERROR_04", "Engelli kart. (Hata Kodu:GWERROR_04)"),
            new KeyValuePair<string, string>("GWERROR_05", "İşlem reddedildi. (Hata Kodu:GWERROR_05)"),
            new KeyValuePair<string, string>("GWERROR_06", "Hata - tekrar deneyin. (Hata Kodu:GWERROR_06)"),
            new KeyValuePair<string, string>("GWERROR_08", "Geçerisz tutar. (Hata Kodu:GWERROR_08)"),
            new KeyValuePair<string, string>("GWERROR_13", "Geçersiz tutar. (Hata Kodu:GWERROR_13)"),
            new KeyValuePair<string, string>("GWERROR_14", "Geçersiz kart. (Hata Kodu:GWERROR_14)"),
            new KeyValuePair<string, string>("GWERROR_15", "Geçersiz kart/kullanıcı. (Hata Kodu:GWERROR_15)"),
            new KeyValuePair<string, string>("GWERROR_19", "İşemi tekrar başlatın. (Hata Kodu:GWERROR_19)"),
            new KeyValuePair<string, string>("GWERROR_20", "Geçersiz yanıt. (Hata Kodu:GWERROR_20)"),
            new KeyValuePair<string, string>("GWERROR_30", "Format hatasu. (Hata Kodu:GWERROR_30)"),
            new KeyValuePair<string, string>("GWERROR_34", "Doğrulama başarısız. (Hata Kodu:GWERROR_34)"),
            new KeyValuePair<string, string>("GWERROR_36", "Yetersiz limit.. (Hata Kodu:GWERROR_36)"),
            new KeyValuePair<string, string>("GWERROR_41", "Kayıp/çalıntı kart. (Hata Kodu:GWERROR_41)"),
            new KeyValuePair<string, string>("GWERROR_43", "Çalıntı kart. (Hata Kodu:GWERROR_43)"),
            new KeyValuePair<string, string>("GWERROR_51", "Yetersiz limit. (Hata Kodu:GWERROR_51)"),
            new KeyValuePair<string, string>("GWERROR_53", "Hesap bulunmadı. (Hata Kodu:GWERROR_53)"),
            new KeyValuePair<string, string>("GWERROR_54", "Süresi dolmuş kart. (Hata Kodu:GWERROR_54)"),
            new KeyValuePair<string, string>("GWERROR_55", "Yanlış pin. (Hata Kodu:GWERROR_55)"),
            new KeyValuePair<string, string>("GWERROR_57", "İşleme izin verilmedi. (Hata Kodu:GWERROR_57)"),
            new KeyValuePair<string, string>("GWERROR_58", "Firmaya izin verilmedi. (Hata Kodu:GWERROR_58)"),
            new KeyValuePair<string, string>("GWERROR_61", "Tutar limiti aşıyor. (Hata Kodu:GWERROR_61)"),
            new KeyValuePair<string, string>("GWERROR_62", "Kısıtlı kart. (Hata Kodu:GWERROR_62)"),
            new KeyValuePair<string, string>("GWERROR_63", "Güvenlik ihlali. (Hata Kodu:GWERROR_63)"),
            new KeyValuePair<string, string>("GWERROR_65", "Tutar limiti aşıyor. (Hata Kodu:GWERROR_65)"),
            new KeyValuePair<string, string>("GWERROR_75", "Şifre hatalı. (Hata Kodu:GWERROR_75)"),
            new KeyValuePair<string, string>("GWERROR_82", "Zama aşımı. (Hata Kodu:GWERROR_82)"),
            new KeyValuePair<string, string>("GWERROR_84", "Geçersiz cvv. (Hata Kodu:GWERROR_84)"),
            new KeyValuePair<string, string>("GWERROR_89", "Kimlik doğrulama başarısız. (Hata Kodu:GWERROR_89)"),
            new KeyValuePair<string, string>("GWERROR_91", "Teknik bir hata oluştu. İşlem devam edemiyor. (Hata Kodu:GWERROR_91)"),
            new KeyValuePair<string, string>("GWERROR_92", "Router kullanılamıyor. (Hata Kodu:GWERROR_92)"),
            new KeyValuePair<string, string>("GWERROR_93", "Yasadışı işlem. (Hata Kodu:GWERROR_93)"),
            new KeyValuePair<string, string>("GWERROR_94", "Çift işlem. (Hata Kodu:GWERROR_94)"),
            new KeyValuePair<string, string>("GWERROR_96", "Sistem arızası. (Hata Kodu:GWERROR_96)"),
            new KeyValuePair<string, string>("GWERROR_98", "İşlem iptal edilirken hata oluştu. (Hata Kodu:GWERROR_98)"),
            new KeyValuePair<string, string>("GWERROR_99", "Hatalı banka. (Hata Kodu:GWERROR_99)"),
            new KeyValuePair<string, string>("GWERROR_102", "Zaman aşımı. (Hata Kodu:GWERROR_102)"),
            new KeyValuePair<string, string>("GWERROR_105", "3D secure başarısız. (Hata Kodu:GWERROR_105)"),
            new KeyValuePair<string, string>("GWERROR_2204", "Taksitlendirme için izin yok. (Hata Kodu:GWERROR_2204)"),
            new KeyValuePair<string, string>("GWERROR_2304", "Devam eden işlem var. (Hata Kodu:GWERROR_2304)"),
            new KeyValuePair<string, string>("GWERROR_5007", "Kartınız sadece 3d Secure işlemi destekliyor. (Hata Kodu:GWERROR_5007)"),
            new KeyValuePair<string, string>("ALREADY_AUTHORIZED","İşlemi tekrar başlatın. (Hata Kodu:ALREADY_AUTHORIZED)"),
            new KeyValuePair<string, string>("NEW_ERROR", "Sistem hatası. (Hata Kodu:NEW_ERROR)"),
            new KeyValuePair<string, string>("WRONG_ERROR", "Sistem hatası. (Hata Kodu:WRONG_ERROR)"),
            new KeyValuePair<string, string>("-9999", "Engellenmiş işlem. (Hata Kodu:-9999)"),
            new KeyValuePair<string, string>("1", "Çağrı merkezinden, destek alın. (Hata Kodu:1)")
        };

        public struct PaymentResponse
        {
            public string ReturnCode { get; set; }

            public string ReturnMessage { get; set; }

            public string ReturnStatus { get; set; }

            public string PaymentMessage { get; set; }
        }

        public PaymentResponse Payment(
            int installment,
            string cardNumber,
            string nameSurname,
            string expiryMonth,
            string expiryYear,
            string cvc,
            string redirectUrl,
            decimal amount,
            string billName,
            string billSurname,
            string billEmail,
            string billPhone)
        {
            var ayarlar = AyarlarService.Get();

            var payuMerchantName = ayarlar.PayuMerchantName;
            var payuSecretKey = ayarlar.PayuSecretKey;
            var payuMerchantName3D = ayarlar.PayuMerchantName3D;
            var payuSecretKey3D = ayarlar.PayuSecretKey3D;

            cardNumber = cardNumber.Trim();
            nameSurname = nameSurname.Trim();
            cvc = cvc.Trim();

            var paymentResponse = new PaymentResponse();

            if (installment == 0)
            {
                paymentResponse.PaymentMessage = Resources.LangResources.BilinmeyenBirHataOlustu;
                return paymentResponse;
            }

            var cardNumberForBin = cardNumber.Trim().Replace(" ", "");

            cardNumberForBin = cardNumberForBin.Length > 5
                ? cardNumberForBin.Substring(0, 6)
                : "";

            if (string.IsNullOrEmpty(cardNumberForBin))
            {
                paymentResponse.PaymentMessage = Resources.LangResources.BilinmeyenBirHataOlustu;
                return paymentResponse;
            }

            var cardProgramInfo = GetBinInfoObject(cardNumberForBin);

            var cardProgram = cardProgramInfo?.CardBinInfo?.Program;

            if (string.IsNullOrEmpty(cardProgram))
            {
                installment = 1;
            }

            var installmentsInfo = GetInstallments(amount);

            var installmentsString = new List<string>
                {
                    "Two",
                    "Three",
                    "Four",
                    "Five",
                    "Six",
                    "Seven",
                    "Eight",
                    "Nine"
                };

            var percent = installment == 1
                ? 0
                : Convert.ToDouble(
                    GetPropValue(
                        GetPropValue(
                            GetPropValue(
                                installmentsInfo.value,
                                cardProgram.ToLower()),
                        installmentsString[installment - 2]), "percent").
                        ToString().
                        Replace(".", ","));

            var totalAmount = percent == 0
                ? amount
                : Convert.ToDecimal(
                    GetPropValue(
                        GetPropValue(
                            GetPropValue(
                                installmentsInfo.value,
                                cardProgram.ToLower()),
                            installmentsString[installment - 2]),
                        "total").
                        ToString().
                        Replace(".", ","));

            var backRef = !HttpContext.Current.Request.IsLocal
                ? $"{ ayarlar.SiteUrl}{redirectUrl}"
                : $"{ ayarlar.SiteUrlLocal}{redirectUrl}";

            var orderref = $"{ string.Format("0:00", DateTime.Now.Day)}{ string.Format("0:00", DateTime.Now.Month)}{ string.Format("0:00", DateTime.Now.Year)}{ string.Format("0:00", DateTime.Now.Hour)}{ string.Format("0:00", DateTime.Now.Minute)}{ string.Format("0:00", DateTime.Now.Second)}";

            var apiPaymentRequest = new ApiPaymentRequest
            {
                Config = new ApiPaymentRequest.PayUConfig
                {
                    MERCHANT = payuMerchantName,
                    LANGUAGE = "TR",
                    PAY_METHOD = "CCVISAMC",
                    BACK_REF = backRef,
                    PRICES_CURRENCY = "TRY"
                },
                Order = new ApiPaymentRequest.PayUOrder
                {
                    ORDER_REF = orderref,
                    ORDER_SHIPPING = "0",
                    ORDER_DATE = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                }
            };

            var orderItem = new ApiPaymentRequest.PayUOrder.PayUOrderItem
            {
                ORDER_PRICE = string.Format("{0:0.00}", (totalAmount).ToString(CultureInfo.CreateSpecificCulture("en-GB"))),
                ORDER_PINFO = "",
                ORDER_QTY = "1",
                ORDER_PCODE = orderref,
                ORDER_PNAME = orderref,
                ORDER_VAT = "",
                ORDER_PRICE_TYPE = "NET"
            };

            apiPaymentRequest.Order.OrderItems.Add(orderItem);

            apiPaymentRequest.CreditCard = new ApiPaymentRequest.PayUCreditCard
            {
                CC_NUMBER = cardNumber,
                EXP_MONTH = expiryMonth,
                EXP_YEAR = expiryYear,
                CC_CVV = cvc,
                CC_OWNER = nameSurname,

                SELECTED_INSTALLMENTS_NUMBER = installment.ToString()
            };

            apiPaymentRequest.Customer = new ApiPaymentRequest.PayUCustomer
            {
                BILL_FNAME = billName,
                BILL_LNAME = billSurname,
                BILL_EMAIL = billEmail,
                BILL_PHONE = billPhone,
                BILL_FAX = "",
                BILL_ADDRESS = "",
                BILL_ADDRESS2 = "",
                BILL_ZIPCODE = "",
                BILL_CITY = "",
                BILL_COUNTRYCODE = "TR",
                BILL_STATE = "",
                CLIENT_IP = HttpContext.Current.Request.UserHostAddress
            };

            var options = new Options
            {
                Url = "https://secure.payu.com.tr/order/alu/v3",
                SecretKey = payuSecretKey
            };

            var response = ApiPaymentRequest.Non3DExecute(apiPaymentRequest, options);

            paymentResponse.ReturnCode = response.RETURN_CODE;
            paymentResponse.ReturnMessage = response.RETURN_MESSAGE;
            paymentResponse.ReturnStatus = response.STATUS;

            if (string.Equals(response.STATUS, "SUCCESS") && string.Equals(paymentResponse.ReturnCode, "3DS_ENROLLED"))
            {
                apiPaymentRequest.Config.MERCHANT = payuMerchantName3D;
                options.SecretKey = payuSecretKey3D;

                var response3D = ApiPaymentRequest.ThreeDSecurePayment(apiPaymentRequest, options);

                paymentResponse.ReturnCode = response3D.RETURN_CODE;
                paymentResponse.ReturnMessage = response3D.RETURN_MESSAGE;
                paymentResponse.ReturnStatus = response3D.STATUS;

                if (string.Equals(response3D.STATUS, "SUCCESS"))
                {
                    HttpContext.Current.Response.Redirect(response3D.URL_3DS);
                    HttpContext.Current.Response.End();
                }
            }

            if (string.Equals(response.STATUS, "SUCCESS") && string.Equals(response.RETURN_CODE, "AUTHORIZED"))
            {
                //var campaignDetailCode = new CampaignDetailCodeManager().GetByCode(code);
                //if (campaignDetailCode != null)
                //{
                //    campaignDetailCode.IsUsed = true;
                //    campaignDetailCode.UserId = user.UserId;
                //    new CampaignDetailCodeManager().Update(campaignDetailCode);
                //    var campaignDetail = new CampaignDetailManager().GetByIdWithAll(campaignDetailCode.CampaignDetailId);

                //    var _orderManager = new OrderManager();
                //    var tempOrder = _orderManager.GetByIdWithAll(order.OrderId);
                //    var codeCampaignDiscount = (tempOrder.ProductsTotalAmount < campaignDetail.DiscountAmount ? tempOrder.ProductsTotalAmount : campaignDetail.DiscountAmount);

                //    foreach (var orderDetail in tempOrder.OrderDetails)
                //    {
                //        var tempOrderDetail = new OrderDetailManager().GetByIf(orderDetail.OrderDetailId);
                //        tempOrderDetail.ProductUnitPrice -= (decimal)codeCampaignDiscount / tempOrder.ProductsTotalQauntity;
                //        tempOrderDetail.ProductUnitPriceDiscounted -= (decimal)codeCampaignDiscount / tempOrder.ProductsTotalQauntity;

                //        new EfOrderDetailDal().Update(
                //            new Dal.Context.ECommerceContext().CreateContext(),
                //            tempOrderDetail,
                //            true);
                //    }

                //    tempOrder = _orderManager.GetById(order.OrderId);

                //    tempOrder.ProductsTotalAmount -= (decimal)codeCampaignDiscount;
                //    tempOrder.OrderTotalAmount -= (decimal)codeCampaignDiscount;
                //    tempOrder.OrderTotalAmountNotRated -= (decimal)codeCampaignDiscount;

                //    new EfOrderDal().Update(new Dal.Context.ECommerceContext().CreateContext(),
                //            tempOrder,
                //            true);
                //}
            }
            else
            {
                paymentResponse.PaymentMessage = GetReturnMessage(paymentResponse.ReturnCode, paymentResponse.ReturnMessage);
            }

            return paymentResponse;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public string GetInstallmentsInfoString(decimal amount)
        {
            var ayarlar = AyarlarService.Get();
            var amountString = amount.ToString().Replace(",", ".");

            var request = (HttpWebRequest)WebRequest.Create($"https://secure.payu.com.tr/openpayu/v2/installment_payment.json/get_available_installments/{ayarlar.PayuMerchantName}/{amountString}");
            var response = (HttpWebResponse)request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public CardInstallmentInfo GetInstallments(decimal amount)
        {
            var value = GetInstallmentsInfoString(amount);

            value = value.
                Replace("\"2\"", "\"Two\"").
                Replace("\"3\"", "\"Three\"").
                Replace("\"4\"", "\"Four\"").
                Replace("\"5\"", "\"Five\"").
                Replace("\"6\"", "\"Six\"").
                Replace("\"7\"", "\"Seven\"").
                Replace("\"8\"", "\"Eight\"").
                Replace("\"9\"", "\"Nine\"");

            if (value == "[]")
                return null;
            else
                return JsonConvert.DeserializeObject<PayuHelper.CardInstallmentInfo>(value);
        }

        public string GetBinInfoString(string bin)
        {
            var ayarlar = AyarlarService.Get();

            var span = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            var timestamp = Convert.ToInt32(span.TotalSeconds).ToString();
            var merchant = ayarlar.PayuMerchantName;
            var secret = ayarlar.PayuSecretKey;

            var calcsignature = merchant + timestamp;

            var binaryHash =
                new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(calcsignature));

            var signature = BitConverter.ToString(binaryHash).Replace("-", string.Empty).ToLowerInvariant();

            var request =
                (HttpWebRequest)WebRequest.
                    Create(
                        "https://secure.payu.com.tr/api/card-info/v1/" +
                        bin +
                        "?merchant=" +
                        merchant +
                        "&timestamp=" +
                        timestamp +
                        "&signature=" +
                        signature);

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public object GetBinInfo(string bin)
        {
            var j = new JavaScriptSerializer();

            return j.Deserialize(GetBinInfoString(bin), typeof(object));
        }

        public RootObject GetBinInfoObject(string bin)
        {
            return JsonConvert.DeserializeObject<RootObject>(GetBinInfoString(bin));
        }

        public string GetReturnMessage(string returnCode, string returnMessage)
        {
            var messsage = ReturnCodes.FirstOrDefault(x => x.Key == returnCode).Value;

            if (string.IsNullOrEmpty(messsage))
            {
                messsage = returnMessage;
            }

            return messsage;
        }

        #region Card Bin Info

        public class Status
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Response
        {
            public int HttpCode { get; set; }
            public string HttpMessage { get; set; }
        }

        public class Meta
        {
            public Status Status { get; set; }
            public Response Response { get; set; }
        }

        public class CardBinInfo
        {
            public string BinType { get; set; }
            public string BinIssuer { get; set; }
            public string CardType { get; set; }
            public string Country { get; set; }
            public string Program { get; set; }
            public List<int> Installments { get; set; }
            public string PaymentMethod { get; set; }
        }

        public class RootObject
        {
            public Meta Meta { get; set; }
            public CardBinInfo CardBinInfo { get; set; }
        }

        #endregion

        #region Get Installments

        public class Two
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Three
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Four
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Five
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Six
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Seven
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Eight
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Nine
        {
            public string percent { get; set; }
            public string total { get; set; }
            public string rate { get; set; }
        }

        public class Axess
        {
            public Two Two { get; set; }
            public Three Three { get; set; }
            public Four Four { get; set; }
            public Five Five { get; set; }
            public Six Six { get; set; }
            public Seven Seven { get; set; }
            public Eight Eight { get; set; }
            public Nine Nine { get; set; }
        }

        public class Bonus
        {
            public Two Two { get; set; }
            public Three Three { get; set; }
            public Four Four { get; set; }
            public Five Five { get; set; }
            public Six Six { get; set; }
            public Seven Seven { get; set; }
            public Eight Eight { get; set; }
            public Nine Nine { get; set; }
        }

        public class Maximum
        {
            public Two Two { get; set; }
            public Three Three { get; set; }
            public Four Four { get; set; }
            public Five Five { get; set; }
            public Six Six { get; set; }
            public Seven Seven { get; set; }
            public Eight Eight { get; set; }
            public Nine Nine { get; set; }
        }

        public class Cardfinans
        {
            public Two Two { get; set; }
            public Three Three { get; set; }
            public Four Four { get; set; }
            public Five Five { get; set; }
            public Six Six { get; set; }
            public Seven Seven { get; set; }
            public Eight Eight { get; set; }
            public Nine Nine { get; set; }
        }

        public class World
        {
            public Two Two { get; set; }
            public Three Three { get; set; }
            public Four Four { get; set; }
            public Five Five { get; set; }
            public Six Six { get; set; }
            public Seven Seven { get; set; }
            public Eight Eight { get; set; }
            public Nine Nine { get; set; }
        }

        public class Paraf
        {
            public Two Two { get; set; }
            public Three Three { get; set; }
            public Four Four { get; set; }
            public Five Five { get; set; }
            public Six Six { get; set; }
            public Seven Seven { get; set; }
            public Eight Eight { get; set; }
            public Nine Nine { get; set; }
        }

        public class Value
        {
            public Axess axess { get; set; }
            public Bonus bonus { get; set; }
            public Maximum maximum { get; set; }
            public Cardfinans cardfinans { get; set; }
            public World world { get; set; }
            public Paraf paraf { get; set; }
        }

        public class CardInstallmentInfo
        {
            public Value value { get; set; }
            public IList<int> available_installments { get; set; }
            public IList<string> available_programs { get; set; }
        }

        #endregion
    }
}