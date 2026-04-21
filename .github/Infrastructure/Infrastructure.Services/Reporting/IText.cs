using Infrastructure.Services.Reporting.Models.CTS;
using Infrastructure.Services.Reporting.Models.Itext;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Reporting
{
	public class IText
	{
		public static class PRS
		{
			public static byte[] GetOrder(OrderModel orderData, string abEmailAddress = "")
			{
				return GetOrderKanban(orderData, false, abEmailAddress);
			}
			public static byte[] GetOrderKanban(OrderModel orderData, bool isKanban = true, string abEmailAddress = "")
			{
				#region convertHtml
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				string body = string.Empty;
				string invoiceHeader = string.Empty;
				string invoiceNumber = string.Empty;

				if(orderData is not null)
				{
					string logo = "";
					var psz = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
					//REM:  - logo imported in STG should be PNG
					logo = $"data:image/png;base64,{Convert.ToBase64String(psz?.Logo)}";
					if(string.IsNullOrEmpty(logo))
					{
						// UNDONE : Fix logo
						logo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAARwAAABbCAYAAACh3jqAAAAABmJLR0QA/wD/AP+gvaeTAAAnvUlEQVR42u1dB3gURRseQUEBFRUbxUITRX8LKqg/KhZQFBQVQRAsYPuRJr1L54J0CC2EhBoSQkglPSQkpEEI6YQAoSf0TmjO/317s8fe5u623N7lEnae532S252dmd3bee+brw0hd16pB+gKcAMEATIB+wCnACfZ/zmAQMAsQC9AC8BdRC+KCqW0OqBmH9/tD1cf5L0ZEDEoYFddPNbaENYQ0XRSUMu6I9d3aTsn/Ek8XpXR1TPh0UVxuXUsnftj7baH8Fzd4es7t5we2BSPNW36aZVCy5Zda3DvhcYoA5QAsgBhgJmA3oBGFfjuVwd8A4gF3FR5X0cBywHtAdXsHM8MBzx3rTBDq4c+LSrH/2vPrbTVzBC6IH4PbTsvgrZbEEGfmuBHqw30pmSAF4cag1fR56YG0JDcY/TSdVoOpZdumH2OKzpJ7xuymt49yJumHTrLHVuVtp/2Xp1I28wJo3cN9DK1/czfG+kPa5Lo3K0FXPsFJy/RI+eumdo6efkmPXS2jCYdOE290w/Q0SG7YJzhtOZfq01t1BvtQ9+as4UePX/7ut82pNCOS2K46/r6JNMOi6NN9S1BeL9C3Av30c8vjS5O2ktj9p6g/f3T6Lvzw+kXS6Jpo2bdAd2qEL49Rpz8MiMJjQLUdyLZdATka3wfewG/A+7WCcd66ewRk8JPrFHBGfT4hev0p7VJ9FefFDo+LIu+ND0ISGOV2WTsujKB5pVepBeu/WtCQNZheq7sltkxJCG/zEP09JWbdEZMLkcKfBtNJm2iY0J30azj582uESP7+AW6IGEP/QUIo8/6ZCCiMrP212UU08+XxtJ7gBCx3dpD19ChgRk0ft8pbszvAjFhX3VHrrNJNlJE9IdvKm0+JYA+O9GfghRoOteoeTedcDTCdcAKwLMOJJo6AC8H30cB4EOdcCyXSRFZm3qs2sZJNSgNuAExDA3cyUkPX3rE0bdmh9F35m6ha3YU029WbDURTw04Py5sNz1x+QZHNDsPn6MlF69z/wuRcvA0/S9cz0/Qt0EK8c86Uq6eJQSDtFN/nK/Z5L8PCAXHi9LGHpCEwguO061FJ0B6OkA/XxZL72VSz/3D1oJEtpm2AHReHquabKSgE472uAqYAKihMdk8Bkh30j38C1is8B7uCML5ekVcIk7MuwW/2jip8S9KDf38UukZkFAQS7fvNS2FeOJ5cXogTT90hhafuUJPgsTB10UgedUcYiSAZiAdbNp9mDt+DJY9wnqWcADae2KskWxqwXj4MSEajvfjxoHnp0Zm0//MCLRKCM0nb6LdVsbrhFOJCIdHkobLrPsAaRVwD3GAmlWAcCZqRTifLbu9pBJLErhMmhyRTVem7qNeoH/5c2MaJ9k8CRO9/eIoTvLBunWGrqWeKfvoKZB2EIfPXqWdlhmlCiSyUcG7uKWQe+Ie+tGiKPrCtM10E7TN1xcDr2/vHmUaS3DOUdoFpCv+c4NxfjS35AL9e8tuhxGJTjiugWINllhoSVpXgaRZFSScnloRjiEm1xcVsOKJNCEsk54AzWvJxWvcsigbdC0DNqXTWXH5tM+67TQCljKlsITCeigJocQzBaSNPScv0jdnhTJJxJeG5h7l2kGkgiSUdOCU6bMlBGQfpk1B54LXo65keXIR/R9IWeLxNQZdSkUSTUNQqkdkFlcxsnE9wkHkAe634x0fUoFjb1cFCOemlnq1TsvjtluaUPO25tOSC9dMOA7LoOenGZdeixMLadGpS5wkgud8Mg5yUo7RWrSe+/sSLLWyjp43a8MWjp0vowdPXzFTyD4yyodmHztPp4CUhQpblHqsWZKcia6e8bSg9ALN3X9CJxwnYbXK9xvN7pcraMzRVURp7KOpeXCJ5SVVI9CToHI2Eaw9qHM5DGZpVCJzehGY/B1gSTU1Moc7h/Dddch07WNjfOieE5dM52whB6xQ06AdXHLh55bTAmlnthxrDQrrtINnaCEoh9vA//cNWVPhZMMTzszYXJq9TyccZ+IDFe/3Ajt/2c8A8IFcUHH9W1WAcC4CmmhJOJ8uiUqxNblQ4vgFzNHxYAl6/Z8Qs3MNwIKExFJ85qqZJQoxNjSTIxEpFJ28TB8Gk/VW8G/Bz9jWsM07aVNQ9qK/DRId+ukIldqOAr80lFMXiTGrqFQnHAb0vH3IAtCDtzmgDaA/IBhwS+XLv02FCfyiwj4uM6fENy1YmB4HfAsIZ1YoW+2EqJiLrQG/OgijVD73Plr7JUyJyMqfHL6b8vBKLaKz4ddbeAw/J8KvufAYjwXx+dQ7rYj7331bAVisTsMSaDeH7GNn6fmrN6zi7JUbNAh0Nn6gCwnIAn+dy9fpTrh+MSiX523N4/7Ojsu12K/WwPF6JO8FJbm8+r7g/1Ny+hKdt8i/0qBrz4kOI5yvFLxzLZkyVQ3pvKqgnx4K2z7MwhXklOcA7sx3SNzODcArLhZR4KPiWQc5YiChuYePUb3cEWXmnA0uQTiESQ+bVfQzTUEfKxS0e4tJGEpLUzaZbwra+dPFyKaHiueM8WNPOGIwwzbvyO67PolawoKEfPpPTA4sqcyP/+qznZMIMo6cpoM3pXHHtuQdpSfBooU4cbEM9DtZ3HGUevjj1pB68BS0l0WtjcOVMAjuN2bPcXrk7BVaXHKODh+91OUREJTocoSDpS7gkMJ+dihoP0NBu5F2zqMnmEXqaRcjG/RjOq3i++zqqAG1d49Msaa7aQj+LqjAfQ/ihtCBTlzn7dlGvc1LM4JoXGEJLQJ9Dg/8jN7K6KDnD+ENwnOW8D0oqIVtvz4zmNPdiPv8GvxxPlwYSceAb8/tMAl/aojKAUfADZrrdR5j4Rh3g35nIYRY7Cm5SMPAAzoFzPuxmQfBD+c7l9fLjJvo6ZKEQ5h+QUk/uFypJbPtcwraHUeqXkH/ozAV3+UqRw6q/aJIm0rjj2BydwBzdDPwjakPBITH0E9mHCiFMaDzHoiziswvoYWll8rhN4jHwvqtgDwsnUeMD90NElMyfcUQzBHGHxtS6cL4ArCSbaRDAnbQ9yHkgh8LOgzuOHiWLgGzfAoEZI5ipDMPgj43gGn+XkEwpxbAuKmYPSUg5eXRZyZupLuPnKP5xy/ScSGZNL34LE0E/VNlUAS7MuHUYSSipK8XZU42JUrSwVWQcPqp+B6PMGW/w8rHEoRTa6j5JH4Dosp9dh7korvxcw+vBFoAv/qWgOTwwHCjfw5Giluq89gYo1Ty4PB1nCSEx9xAWkktPg1STyLng/M41HHfVsgFYloMXwAzfVxhKUdsWks4ny2NoZlANN284ml0QSk3vi15x0C5fZYm5x7RCcdOwsGSpbCvjjLbLVPQ5sIqRjaNVVjo0PL2iaMHJkU4Qjw9YSNNKDxB0w6coXWGreGWXZF5JTTv2AWr4KWcdvMjyp3LPHyWtpiymdYdsQ7STWyh7gmFdAREev8NQaHRIDWtTN7H1VsCZJN95DwNhqDPgRBe0Rc8nYXjenjkepq49yTn2aw14bwAcWYZQJz8mPHeUQpLhPQbSbqEownhbFHY17cy2z2uoM1jjv5ld2LBvDwJKr7D+c4Y3EcLI2QTDqaX2A1R4eNhOYWfUZeSA97EtoCSBxIT+rfEgIQgPj8NwiEG++8AfY2/QG+ygWaBVGGtTU8gIvHY6kB0uDDHjlbAccfCsgrvewUErw4AwpsOUheOYxuY8nXCsZ9wNhLHxPXkKWwXgzsbVAHCGaPi+ytiy1uXIZwWsGwJyT7CEQEuqzjdSVwB91kK78wxKpeHg0MffkbJBv/Ojs3j8tdY6u9FcKzr55tCk/adpF9CeglMjTEyaCf9cU0i7emdQB8V5NYxOuJtpp2Wap+GAnVLO8HbeQMsI+OBPIX3FZ91UCccDQgnRmFfXRzULgJTig4g2qfGcFZB/59rKhTxrZ01wA8WlCccDGsYCUsbJIhR8PdpsBZNj8ihmaC3iIWlTjWQWB6EZdAO0LPgMSmgRIDtvmoI4j6nQLhEbEGJzbQSPGr+tcoi+U3eklXuOI61E+hcuHw4oDvC1Bmo1FZDNCgtveYWDFLZcav3FZepE44WhFOosK/3ZLbrRtS79GM61MnMwa+ylJoq9GGISc4c5G8+yUGYrEo42SLA7JsBv+o8PCDZFUoV20FvYYC4J6yDlithHVuIA3LBpQlmDkyCNnYWn6EfLIjkUpCqIQNcovlD7BYSj1loArTPZ/7rBWZ27HswRLhbkqLwWDeIieKV2jyQaGfAPUbmHadRAFv3FburWCccOwmnHpEOExBDbuTy+0SbeKJswN+Al1yccGaquLcMZ0tzP6/bHjIRlLR8DBFO5pT9p0B6OcMhdf9puiG9mM4Ey1HArsO0O0tmNTpol6mOHDzHyMEdfFksSSdK8RvEd6FSuZqVGKvVqQdMfQdC4i/zpFwB1DtlP3duRVKRme5nRdI+03Uo1dm6p+gMnXDsJZw/ifJgwuoy28Z6e4i2wYzYHno7/8fFyOYdojwRfFlFkGj6vlOGbYUnuaRa/KT7JzqPpgHRoGXJP+MQ9z9iNUzSxpOMyt1V8D9/XA6+ZQm0/gQLzwfzI+wmnOpAkIGZh2kblnuHX34hcdYGiS113+2+cQmHliys8wSY2KNyj5udWwZWML6NJyEglT+XCAGltu4pcscBnXDsIJxaTFmpNIOektKLOC6SeidzXKxofU9tYkzgrnT8QypisMlAODjpfHccpL+D1PAaKElR19IK9Bdo+RkLznW4DMLlVDgstR4csZab1BE5x7nJisfxrxSGwNIGJ/QX4Ln85TJtlLuop8H23oFdIEbDUmjxtj0mB0Fx///bYDTPo9Oe+Fxy0Sn6n+mBpsRe/HH0MQrafcTqPYWn79cJRyXhoGPeChX9jFbYT3WVeg0lOAj4hdi/TYzaslzFmBMVSIqaFiATw3ZGKELEgHLYI3EvrQ+Sz1ywJuGx6Pzj3KRERzy+XiL45Vi6XgxsA69FRawcZbFcYBqLZ8A/6GuPrXRObL5JvyTuPxLIEvU7q2AZZml8fN5jDLHgj6E+Z2HcHqv3FJa2TyccFYSDycz9VfSBup7nVbzjGAF+njg+dwzGeb3p5PnbQYUO7BIxBp1WSIHllAFJwxoico5RH9DhuIEi1Zv5v6DCFSf1n7B1ik9aMbV1PQ9P0JVwcU8gQQyHkAUtfWZQ4poFy8DxLNQBlcGWxrAUlk7WxvfVcmNysS5ggsfPqOiOhKXXAN90GgxSjqVrQlOKdMKRQTgPApoBvgN4AK4Q52TQE5bOKszFaoB99HHS3K2n0LnRYTlulJSEwlJDwp5Sagvx4LC3HrZh6bt2e7nJjkm4PCC2aQt4AdtqA6/H+igx4ed+bImjBVDXhG0OBQdC/PwjbLZnaQyjgzLMPqMuxx9isPD/YQHGaztAknf8vBg2BYwBK9UP0BbWs9RecMpenXCcBPwVb2vnu96eKHf3V4tRTpi7virGFU4qeHvirQWlBgCVg+6e8TaljPHBmVavReUzH4aAn6NhMvddl8zpTHCZ1XFxDLeEeQUiz605A1oC6llQn4Rt/umbxh37BUIf5N7T1C3ZNCDjMJ0YatwB4t254UA2hRzxRIJ0h23H5JbQuPzy1wYm64TjLGgVwfwCUef2r4Ygv3LgvFWjDHdYjhslJTav1BAL8VBiRMNEi8g+avqMSyepyf8JTFJLbSHWgFWLswJBRLi1Ojz6+ciTfjq6R5tdN3ijUTHdG4I+pfrgEQPSy3ewk+gPq5JMG+jhcu8tsH79vDqJeoGZfB2Y2L2276P+4G0clnX7mQQkFeqE4wSgKfoBDd95/IXvwfxrHDluzIP8uAPmLIZfnCEulONGSQFiMUTDpJPCClAgS6V/uB8COhtDTNTbs8I40gjPPma6fmmC0fSM56X66uW9TZJsMMdxcOYRs+tGbs7gzn0Dyd7l3BOPLUAir7sZI82bgRIac+C4gQ8OEuhm8D1aD4QzIiCDW6rhuZZTA+nfYO1aHpWtE46DUepAb18kHoyO9iPKIsuVYLEDxryFuFiOGyUlEggHlw5SWA0K47rD1yvbuwmit/E6vH4GC294GfYql+prEyy/MDJdqv25kKdGeN1U5lD4Ieh05NyTED1haYbXvgcpMNbAmFG6Q2UxngsD/VQIkBtKOCMh3APJDs3oX7pH6oTjQOwl8nMM21seBvxIjGlPz2p4Dxin1EjDcfYnLpjjRkkJzzpqAFA56L5C+Za533tt4679H1smodQgp69BfmmSbaPfkPCaFdv2GmOtQEqRe0883me5dj6H/dXngsXLM7GILoLg1Gnh2XQDKLxnwN/RIEH5gcVuydY91B2Sfo3akEwbvPC9TjgOwAZGAhVRUIp4lBi3pUEJ5bSd9zJUo3E1Iepy3HzqSi7RYZmHDWHwSy4HPjDx7lJANi+DAjgUvIHx2s5LYkwWJDl9zY7KlWy/79okru5KiPXCv0Gw/EEPZFQ6h2bKuyceTScaU6i6gSTGHwuGsaOi/GNIw8Ev4zbuKObO/QQ6n/WwC6ku4WjvvetSE4R5RP/CJAU19xSswRjuBqSo6HuBiz1LErrrsCEEJqpc4JJILuH8Bblj+Ouasq15p4A1SE4/X0ikmmjzTyj1AilkTlQe+PZsohtBqY3XNWHE4Q7Sidx7wmuRTJCs/IBU+eMTQU8j9hd6FnRQPUDJjPma58C+WTrhaOO7spGZre8irltwu2EvFfdXoEHf44kL57hRUoKBcILALCwXwzbtlE044yDAE69Zn3KAM5vjhN4AClg5/bwgigQXohNIS3y998CM/QiY2vnPny82SlK/gcld7j2NDzI6DCIp4ueBG9JARxNE75WIZn8DJLhGzbrrhKNCERwLmAP4whUnhcRyaxZRnmPHnvIqUZfjpo0rPsDAnQcNm3ceonKxMhGjq70l00eg0tcf4rPwmiHMXI3pJOT2M9DXug7nS5B++HqovG0NO4Lyn0cGGAkRrU5y+/qMkRTfbr2RPrJJtVFzXcLhs/B9ZAHorNeKGKOrUXlaWRNbiZc3BxQ8G0zmrjZuSW2Om8mu+vD80w8aNqUfpErAL48s4XUI/pwHSldh/VbM5PwT+MfI7cOTKYDLbdsyyocuhfgmvl5z2EGiCxAF/3kDSFO1QDJBaQrbkOrHD5ZTdSGBO7ZtACdAf/gsx0KmE445niZ3VvFW+HxqquxntorvYpcrE7tf+gED6jCUoP2CKKsTcFpYllndFQl7OYkHdSFLgCjk9uELSy8kF3H7GMkurLcSSMUHSEZ4rC3bL+tH70TJfkZuMoY0YMpSv1TjsfdF+6TrhKMTjrhMVPBsrqvs479E+X7gFZLjRkkBnYoBJ7cS/My2iBHjUzB5i+t2ZUGRmPZCaT/DWGwUjwchHcUiiAiXum5isDHJ+xNAIj7g4Wyr7kvTN3N1vwNrFH/sJyCqu5i162PITDgYTPT9wayPktrdooRfOuHcmYSjJLveQRXto2d1cQWa4B1WfJL3GXyS91Ml6LgoupyH8cuQ9FxcD6UPPn3pOPDUVdrPktgC2gCCPd8whNAvQVE8C7x/5V7bmO3aORASflmrM5XFT6HFyQOCNfnjC2FJ+HdQZrn6nhBjhcRTfaBOOJWJcNCn5i+m18D4Ji3y1ijZFz1WRfuepBLluFFS1iXtM6yDOCG5WAkhCrWGrBHt3RRgsW4XltC8Geh8lPShBQYzpTNm+FsFim5LdVpONUo3nwGBKrn/ToujuUTz31QST+PxjHD+met7xxEO5j8+KhprLnPqU1tQ+lCSX2e6wvY7q3j+FZrjRklZA4QDoHKxAn7lq4uWFWh9EtebG5nLbQWMupsJgbuokj60wnOTjD45PT0Typ0b7Gu0nNUZupZTQitte3XiPjrdP93lzeJduo2n585d4gjnatk1+sfAuXcM4aAJe6uNMfsQdWEHSs3iHytoW22Om76VZS26aluRAUCVAEMHuJileRHcrpn4f791Kabz3glFtOUUo/TwFjjoKW1fK0yCZREuf2rAkskNLFD88cUx+fQhNu7eK7epbr8t6HQatujhsmTT+IWetPTEWSosZWXX6Ztt/7gjCOdHGeO+wvQxTWS0dx9ghsLncoKZ0WUbcUglzHGjpHglFBm8wJIkF7PCc7hlCjrb4f9/gZMcSjF1QMGKn7FOd494037hC6PzqZL2tUYn5mPTBHQ6HhADtTJ+L32DmemfB1LEz2rbXhic4dLSzYcdh1BL5bf+s6s84TxCjPlflMQcbQdMAHwGeAvQEvAGMaZ1mEuMe1UpfS4zFYy5N1HnSIljG+FAaJoh0DOh0IDKULn4iaWOaDEpwHSs3Vxj4ONT4/zoAFCqVhvoxVl5BoHCVknbjsAyWC49M8GPG99/IW3GV8viTGlSZ0J0uT1tz4edQF2ZcF547Sd6/fqNcoTzyRcjqjzhrCAVH4CKJuqnZI63IVGX48YZyNHyi/HYWmjwgEkpF92WbzXpRfhjaE1qxvQlfOxRF7AqKWnXkZgZls2Z1PnxISGiZGZvu3M373R5hfHEad701q1/TWSzel1kldfhtCXKE4s7AkqUxf4uSjaaE86y2AIDgMrFgsg8+grEGY2FmCrh8SGCUATczRLrKWnX0fhyye1g0Jag5F4aY3+bcyAPcmWwUnXoNJyOGLuMftV9wh1hpdrpApMUswnWkjnel12EIJ1COEuBcJbA5FOCkRt3cApjd9DP4OdhYPGpzUzltYYaAx6fhmWMG0gWStt2BHpAHh8MHkWr2d1sr/HWM0M5/ZI97c7aVDkIRxmqhoRzugInKPatJEvhMBcmG80JZ3FkgcE9Kp+qxXfL47n9nnCp0mVxLHULzabPslikh8ESNBTIyJ727cH8iFz6/pxwk/l7uN8OOsgn1bSneRNINTEtaLfq9mduTNcJx0V1OM0rSNI5zxTOSorhTiKcRZF5hoWw/FGKGSG76assNw5KDj+CeZk/N3tLDrfs4iPHO0Oy8/kRyvuwB6P9d9KG43xNzn8TNu+6fQ4kND4iHK1rv0AyLTV9GCDkQScc1zWLYwDjVOK4fMVioIPhqyrGeUcRzrzwHANKAnIxNzyXdl0ax0Vk8/tMjYHJbaku1kPph6sHk37Q+lSqpC81MAARtpuzhVMMc4m6YOn0DyztLNV7TZBM7EXwOp4A4RdK+prul6oTDnH90IZniDGJ+E0HTsogYgyhIDrh2C5zwnIMc0EikcIsWCp1BwtVvZHGROpIJB3mR9KZIVk2rxvpl86FNpi8ksGaNWBtMpXTpxLg0ugTGA9PhI+MXEf7QhCm1HW9wdr2IIv3wpQWbdxC6FggUDl9ToXN/HTCqTzBm0g8mO7hnIaTMZeFIthT7ijCmRWWZZgNEoAlIMkMAg/ituC/UodtTnfXAG/6CgRqjoFIbmvXWUIvUNwK001gJHenhVF0PFi7lLQjxMzQLNrXK5Fb2t3Dwi1QP/MJRHgbgAjltjMdoss/BF+imoNXm+6xGaQq/Q4IdlpgptXrpvgk64RDKl+0OHoM47bDocQYg6T0Xi8zj+DPNPLw/Zv54Lgqtmv58MFHZco/oVlXEZMDMspA+rjWbVnc9dZuwTfrjVj/731D1nAApeu/bxqCb47wTS/j6yvFzNDdV7/3iL/+1DjfW3y7iKfH+92CCX/jB8/468N808pguWPxeiCnst9XJ10D5fT1l6dtvvnAsLWmNh6BsXZcEHljalDmVbXjw/tvPy/8Bii7Te3WHrL23+cm+d/6DNoGienaKL8dZXgfXP3128tgkl6tYtiH78VDClGNVM5yD1Py/sk8doPZBMNfddymZgcgjhijt0cC3mOEpRe96EUvetGLXvSiF73oRS960Yte9KIXvehFL3rRi170ohe96EUvetGLXvSiF73oRS960UuVL7hVy1SGt/XHcUeU2sS4Q0bDSjr+BsSYr+hDcjv1bAvAvZVg7BhyM4Ohz5348qELMx8eMFifi1W+PAbYT26nW+1cScbdAbCe2M6JjeEtYYB+xOhp74plvmC8kRq12ROwW4AmKtsZL2pHJxwZpRr79WtFKm9ohyPLJNEk3evi40XpJYGo2xNsHFG2O0dlJZxBontvqbKd5aJ2dMKRUboJ7kePrypfxPt37XfRcWIALsbJWdrHHVO/lgLyiDGuDn+NLxDrO6w+rhOOTjiOKpN1wrFZGrPJis8HcxH94KJk42WBPLKY3qO+Fcn2LUaowh1Yr9kxAXXC0QlHsqzTCUey4E6i37jYRBSWsaT81j6/K1giP8SIB8lmjL6k0gnHkSVVJ5xKXd4TLaMuAt5R2VYNF7w/nXCqGOGc0gmnUpd40Uvfu4rdX5UgHBQhcfvb9uzX4EknEM79xJicHPt8lxhThWpVHmHtKy11RQ9MC8JB3w70T0KfDzXmxobs+vZMx9BAw+f0OGsT236dOM8P5V72Hd2jcbtvi76/rRVECvg9t2Nzqq6Neg8I5sCbMt83W4SDS0bcygh9pNoqeFecRjifsy9FnIgctfjpxJiys5rGhPMRIIqtj8VKvQJAfxWiLL68aMLELWRuCNq7yvrqa+PlfpCdn0aMSdOF48kgRiuGEO0ttDGE1ce+P2bHqrMv8pSKSYAvHjpUZhHLlhO0qgyQ+ZyK2TXxgmNt2MsqtuCgH8oyYjtxfG3RNR/K/I4+BfiQ8nuL4fuzCNBMg4k+W9T2Fw4klWmsD7RutWDH2rN3RjiG64B57H3gSyPABnZO/PzdAQ8rJBwk8AmAwxbm8XZGPhVKODWJuXJUaueCOhoQDk54D5l9ItnVl3mT6LQkZ//uPCsv9UtEmX9GdxuEQ9kXX4O9UJauT5W4n1cFJCGFPTJejmLBy4dS3xgRKVt7Vg9rRDj1GOlL3QtOvrF2kkCmoL0LDpCgLBEO4ntiTGdraweRfwTv2ymJZ5FlQzISEw5KU9kS7eF336eiCAeZNkIkBcxnImBjxta/AooEdQKI9cTicggHr90kermWMGkHH1hzttbOF0kXUiL+z6JfaRzzJEZCXYlx90vhl7HGQhu1GDlEWfh1WgFYKsIbEoSDv2ZCXxV0IMsVvGTLbNzP60zJKVR4zmO/1Lj06cw+XxZNrNdkEA4iRNDuAkAnQGtitDgFi+59pQaE85Do+eP37gv4jX0/+J17s3eQfxdbqHzZ7xe9CyEOXjYJCWcnm9RlTFr7nt3bRtGkf1kwr0pYGz0AAy08/+UyCAdXBEfZ/wmMONCPrBd7V4WbCSAZtqoIwhlDzHeUbG3lYpRqEgV1v7WDcAaIxMb3bCwlIgV1x0lMTuFmeHOtLDHuYi/AYjZZbJX2KnU4QsLZwSSIA4xQhaWBjTE8ICKHQiZ6WyqNRc99vw2dlVhawu/U2m4cKwT1blipJ5dw8LlvFtQ7wn7dLZVGbKJ+bgcBNBWNa6YTCYf3uLZ0f8L5dpD93WJFgnETkfOTEoTD/6B1szLGZwWEJF5WO4VwHibmHpU9JRp4SrDO3KGScGqLRMjfJfp8TPALftqGniJW0GY40WZrFy0IB3FShU5iKFHmbPaSaGn0lwzCiZS4p4cE0gZlkohawvlAUOeWjR82rUob0bhGO5FwTrPJbU2VIIzfumJDR1ZDVLe3BOHg3PxEYpxfiqSsJjIIRyuYNbxX5iQNEwy2vgrC6SM4j5tjyYlNEeo/2lk434KNh6/zmkYvkVaE01NF34USSz+p51Qgg3D6y2hzl6D+AjsIx09Qx584vrwvGtcQBdfWILa3S7pfgnCk9vjaKqgbJVE3RlDXIEE4UTLuDY0+xyXeAYcRTpjgwxyZX4ZwB8nOKghHOCk8ZPYp9BQdKvGADmn40mpBOPtFFgk5pYGo314yr/tWdN0TGhCOcBm0SiXh4P2fI871hXlBNK4pCq7tITFxttlJOKtFekFbxVNQd6kMpbGc4i+4xlMG4Wxkc1UpCsSEIzSb7WIKPCkIlX59VBBOnuB8tsw+hb+ybhba9HHQr6cWhBOvot+Oon5bybyuiei6DhoQznpB/bUqCUds+XveCYTzkKjPJQqu/c7BhLNcwbjcJchJDeEIhYZYZ+pwrtopInVVQTgn7OzTUixLnOD87CpAOL2Jui2WxZO/h4sQzkeiOrWcQDioHigh5r5KcsuTbCILrZHbqxDhjBUJGk4jnCvE3ALUVQFQ+VRTBeEIteTuCvv8ilhOhJQuaHNyFSCcX0X9yvX0ri66ro+LEM4XIoWxs8oqkYLUnrQSg6sQ4YwWXJPjTMIRTv5OGn3JUoSzS6Uiz1aJVqGLcmXCEetimqpcRnztIoTTTlSntpMIp6eo3791wim3pIp3JuEkCz6MchLhCJ2fvDTqU6jDCasChNNW1O8HMq97Q3Td2y6qw2nlJMLB7+uYoF90x6inE46ZIcDbmYQzR/AhwUmE019wHiUsLVI2Cn1W0BpybyUnHNRxCGPLhqmYFGVWnkNFEA5+xxdV3I8WRTx5Yohyq2FVIpx7RHrUn51JOB+LDrzhBMJBhyhhjEl3Dfp8VXQfP1VywsESIVJ4SvlIVWP1+Gu2WKlXEYSDJYSYx3w5Kzcwkm6eaIy+RHnkf1UhnK6iH6VHnUk4dzGlEX8Ag93qyNAT9LWxDpcT2iCMo0KRt76Ml6a3hPI0SdAmWicaSihXP5Xx0r8vemBNnEg4n4j67idRv7+ofnsXIxyxqX+iRL+ot3pRI9J5iZj7AfEk/q7M69HbPaASEA4uGZvbaAvJ5YAMlYZDY6kw343QJR5DFl63cDGSAm77wEdir7KDcFDKOUvMneMs+YxgfAnGXR0it+N+rJEEvjy3RG2+ZaHea+R2Uiap+BqxX8tGctuk+x9i9Lyt6yDCEeumbrKlozjiuQ5Tht6SIIaKJhz8cYsj5eOb7rXQHi650GXjJNFur6vWxNxMziOQGPeiwn74sJkHGdn1ZPd/mVQOPxxeTdHVwrIR8+tki1QP9SuCcAhbx4lTFOxlYnkwk3zEofaT7SAcLOibccHCw4pkL0EqKZ8jZy2xnWJgnIUXKoMxOXpUphN5EbjCki+6BsdUSsxN+44inDrE3LWdD7CNY88oiZT3pYomtq1AFUU4hE3qI6L655gScymTIoQ/RKXEeoCnmvIM+0G1xw8s3YrS2xUIZ7Vgzpxi70kIm8vid/hzBXovhyTg+sDCWtcSUHL43g4djljUTZLR50n26y4n6TVa264T6T2GRhN5sWMfENsOkh4W2tGKcHglH+6qeEXinq6wl14q50tFEg4vNebI+M5Rmm1BtC/4DvUi8nMMCWPT+tpQOLsC4XzIlq628uvg/GynUNHusIx/+DC7EKOpbDfTryA7ogULLVqdZbzQbux6byvLJGuTGtMRYB6Rw+xlSGJfxncy9Eri0oItd/IFE/UEe4lHE/nJvIhg+bSGPYtS9mwWWFl68sp43kN1qEYTpQEjsmh2XyVMRA5ky0659zSS3N4aVs5WzH0F92LJkbAmMffIbSGTRNG5MVwgLV5hP2aebInj6FKdTU7MJ4RBj5jk6jh793YxyRIn+y8ypaw+rJ0o1qat0kvwvKQCewcI2rXkt8Zv9TtKoG54lNVF6SaNzV98f7vLVJa3F8xhb6I+je1Pwnb+DztTG/k8pxOXAAAAAElFTkSuQmCC";
					}
					//-
					string positionItems = string.Empty;
					// - Headers
					positionItems += $"<table class='pr-2' style='width: 100%;font-size:12px;table-layout:fixe-d;'>";
					positionItems += $"<tr>" +
							$"<td class='smCell'>Pos<br/></td>" +
							$"<td>Artikelnummer<br/>Part Number</td>" +
							$"<td>Bezeichnung<br/>Description</td>" +
							$"<td>Anzahl<br/>Quantity</td>" +
							$"<td>Einheit<br/>UOM</td>" +
							$"<td>Preiseinheit<br/>Price UOM</td>" +
							$"<td class='smCell'>UST<br/>VAT</td>" +
							$"<td>Einzelpreis<br/>Price/UOM</td>" +
							$"<td align='right'>Gesamtpreis<br/>Total</td>" +
						$"</tr>" +
						$"<tr><td colspan='9'><hr class='hr-2'></td></tr>";

					// - update TotalPrice
					orderData.PositionItems.ForEach(x => x.TotalPrice = x.StartAnzahl * (decimal.TryParse(x.Einzelpreis, out var y) ? y : 0));
					// - items
					foreach(var itemData in orderData.PositionItems.OrderBy(x => int.TryParse(x.Position, out var _x) ? _x : 0))
					{
						positionItems += $"<tr>" +
							$"<td class='smCell'>{itemData.Position}</td>" +
							$"<td>{itemData.Artikelnummer}</td>" +
							$"<td>{itemData.Bezeichnung_1}</td>" +
							$"<td>{itemData.StartAnzahl.FormatDecimal(2)}</td>" +
							$"<td>{itemData.Einheit}</td>" +
							$"<td>{itemData.Preiseinheit?.FormatDecimal(2)} {itemData.Symbol}</td>" +
							$"<td class='smCell'>{itemData.Umsatzsteuer?.FormatDecimal(2)}%</td>" +
							$"<td>{itemData.Einzelpreis?.FormatDecimal(3)}</td>" +
							$"<td align='right' style='width:100%'>{(itemData.StartAnzahl * (decimal.TryParse(itemData.Einzelpreis, out var y) ? y : 0)).FormatDecimal(3)} {itemData.Symbol}</td>" +
						$"</tr>" +
						$"<tr><td class='smCell'></td><td></td><td colspan='7'>Ihre Artikel-Nr/Your Part Number: {itemData.Bestellnummer}" +
						$"<br/>Liefertermin/Due Date: <b>{itemData.LT}</b> ins Lager: {itemData.Lagerort_id}" +
						$"{(string.IsNullOrWhiteSpace(orderData.RahmenBezug) ? "" : $"<br/>Rahmen Nr.: {orderData.RahmenBezug}")}" +
						$"{(itemData.StartAnzahl == itemData.Anzahl ? "" : $"<br/>Geliefert/Delivered: {(itemData.StartAnzahl - itemData.Anzahl).FormatDecimal(2)}")}" +
						$"<b>" +
						(itemData.MHD ? $"<br>Mindesthaltbarkeitsdatum beachten: {itemData.Zeitraum_MHD} Tage ab Lieferdatum" : "") +
						$"{(itemData.COF_Pflichtig ? $"<br>Bitte CoC-Bestätigung bei Lieferung  (Werksbescheinigung 2.1)" : "")}" +
						$"{(itemData.EMPB ? $"<br>Bitte EMPB - Vermessung nach Zeichnung den Teilen beilegen und an die Technik senden" : "")}" +
						$"{(itemData.ESD_Schutz ? $"<br>Bitte liefern Sie ESD gefährdete Bauteile normgerecht verpackt nach DIN EN 61340-5-1 / EN 61340-5-3 an." : "")}" +
						(!string.IsNullOrWhiteSpace(itemData.RahmenBezug) ? $"<br>Rahmennummer :{itemData.RahmenBezug}" : "") +
						$"</b>" +
						$"</td></tr>" +
						$"<tr><td colspan='9'><hr class='hr'></td></tr>";
					}
					var sum = orderData.PositionItems?.Sum(x => x.TotalPrice) ?? 0;
					var ust = orderData.PositionItems?.Sum(x => x.TotalPrice * (decimal.TryParse(x.Umsatzsteuer, out var _u) ? _u : 0)) ?? 0;
					positionItems += $"<tr><td colspan='7'></td>" +
						$"<td align='right'>Summe:</td>" +
						$"<td align='right' style='width:100%'>{sum.FormatDecimal(3)} {orderData.Symbol}</td>" +
						$"</tr>" +
						$"<tr><td colspan='7'></td>" +
						$"<td align='right'>Ust:</td>" +
						$"<td align='right' style='width:100%'>{ust.FormatDecimal(3)} {orderData.Symbol}</td>" +
						$"</tr>" +
						$"<tr><td colspan='8'></td><td class='hr'></td></tr>" +
						$"<tr><td colspan='7'></td>" +
						$"<td align='right' class='pt-2'><b>Gesamtpreis:</b></td>" +
						$"<td align='right' class='pt-2' style='width:100%'>{(sum + ust).FormatDecimal(3)} {orderData.Symbol}</td>" +
						$"</tr>" +
						$"<tr><td colspan='8'></td><td class='hr'></td></tr>" +
						$"<tr><td colspan='8'></td><td class='hr'></td></tr>";
					positionItems += $"</table>";

					var kanbanTexts = isKanban == true
						? $"<p><b>Bitte beachten Sie außerdem folgende Punkte, welche unbedingt einzuhalten sind:</b></p>" +
						$"<ul><li><b>CMS verpflichtet sich, die via E-Mail 2x wöchentlich abgerufene KANBAN-Behälter zum vereinbarten Anliefertermin, jeweils Donnerstag und Montag, als volle Behälter anzuliefern.</b></li>" +
						$"<li><b>CMS schickt innerhalb von 24h eine Bestätigung.</b></li>" +
						$"<li><b>1 Kanban Behälter = 36 Stück.</b></li></ul><br/>"
						: "";

					body = HtmlPdfHelper.Template("PRS_ORD_Body", new List<PdfTag> {
					new PdfTag("<%bodyLogo%>", logo),

					new PdfTag("<%bodyPSZAddress%>", "PSZ electronic GmbH • Im Gstaudach 6 • 92648 Vohenstrauß"),
					new PdfTag("<%bodySupplierAddressAnrede%>", content: $"{orderData.Anrede}"),
					new PdfTag("<%bodySupplierAddressName%>", content: $"{orderData.Vorname_NameFirma}"),
					new PdfTag("<%bodySupplierAddressStreet%>", content: $"{orderData.Strasse_Postfach}"),
					new PdfTag("<%bodySupplierAddressCity%>", content: $"{orderData.Land_PLZ_Ort}"),
					new PdfTag("<%bodySupplierAddressTelephone%>", content: $"Tel.: {orderData.Telefon}"),
					new PdfTag("<%bodySupplierAddressFax%>", content: $"Fax: {orderData.Fax}"),

					new PdfTag("<%bodyContactLabel%>", "- Kontakt"),
					new PdfTag("<%bodyContactValue%>", content: $"{orderData.Name}"),
					new PdfTag("<%bodyTelephoneLabel%>", "- Telefon"),
					new PdfTag("<%bodyTelephoneValue%>", $"{orderData.Telefonnummer}"),
					new PdfTag("<%bodyFaxLabel%>", "- Fax"),
					new PdfTag("<%bodyFaxValue%>", $"{orderData.Faxnummer}"),
					new PdfTag("<%bodyEmailLabel%>", "- Email"),
					new PdfTag("<%bodyEmailValue%>", $"{orderData.Email}"),

					new PdfTag("<%bodyBestellungValue%>", $"{orderData.Bestellung?.Replace("\n", "<br/>")}"),
					new PdfTag("<%bodyMessageValue%>", $""),

					new PdfTag("<%bodyOrderTypeValue%>", $"{orderData.Typ}"),
					new PdfTag("<%bodyLagerValue%>", $"{orderData.Lager}"),
					new PdfTag("<%bodyMessage2Value%>", $"Komplette Bestellnummer muß auf Auftragsbestätigung / Lieferschein / Rechnung immer angegeben werden!\r\n"),

					new PdfTag("<%bodyCustomerNbrLabel%>", $"Ihr Zeichen/Customer # :"),
					new PdfTag("<%bodyCustomerNbrValue%>", $"{orderData.Ihr_Zeichen}"),
					new PdfTag("<%bodySupplierLabel%>", $"Unser Zeichen/Supplier # :"),
					new PdfTag("<%bodySupplierValue%>", $"{orderData.Unser_Zeichen}"),
					new PdfTag("<%bodyDateLabel%>", $"Datum/Date :"),
					new PdfTag("<%bodyDateValue%>", $"{orderData.Datum}"),

					new PdfTag("<%bodyShippingLabel%>", $"Versandart/Delivery Term :"),
					new PdfTag("<%bodyShippingValue%>", $"{orderData.Versandart}"),
					new PdfTag("<%bodyPaymentLabel%>", $"Zahlungsweise/Payment :"),
					new PdfTag("<%bodyPaymentValue%>", $"{orderData.Zahlungsweise}"),
					new PdfTag("<%bodyPTermsLabel%>", $"Zahlungsziel/Payment Term :"),
					new PdfTag("<%bodyPTermsValue%>", $"{orderData.Konditionen}"),

					new PdfTag("<%bodyFreeTextValue%>", $"{orderData.Freitext}"),
					new PdfTag("<%bodyMessage3Value%>", "Wir erwarten für folgende Positionen Ihre verbindliche Auftragsbestätigung mit Angabe der Zolltarifnummer und des Ursprungslandes je Position:"),

					new PdfTag("<%positionItems%>", $"{positionItems}"),

					new PdfTag("<%bodyTableFooterText1%>", $"{(isKanban == true?"Bitte liefern Sie ESD gefährdete Bauteile normgerecht verpackt nach DIN EN 61340-5-1 / EN 61340-5-3 an.":"")}"),

					new PdfTag("<%bodyTableFooterText2Label%>", $"Wir bitten um eine Auftragsbestätigung bis:<br/>Please send us your Order Acknowledgement latest till:"),
					new PdfTag("<%bodyTableFooterText2Value%>", $"{orderData.Bestellbestatigung_erbeten_bis}"),
					new PdfTag("<%bodyTableFooterText22%>", $"{(string.IsNullOrWhiteSpace(abEmailAddress)?"": $"Wir bitten Sie Ihre Auftragsbestätigung an die <b>{abEmailAddress}</b> zu senden.<br/>Please send your Order confirmation to <b>{abEmailAddress}<b/>.")}"),
					new PdfTag("<%bodyTableFooterText3%>", kanbanTexts),
					new PdfTag("<%bodyTableFooterText41%>", $"Bestellung wurde elektronisch erstellt und ist ohne Unterschrift gültig"),
					new PdfTag("<%bodyTableFooterText42%>", $"Für unsere Bestellungen gelten ausschließlich unsere allgemeinen Einkaufsbedingungen."),
					new PdfTag("<%bodyTableFooterText43%>", $"Einsicht möglich unter www.psz-electronic.com"),
					new PdfTag("<%bodyTableFooterText5%>", $"Wir sind gemäß Art.13 DSGVO verpflichtet, Sie als Geschäftskunde über unsere Datenschutzmaßnahmen zu informieren. Unsere Datenschutzerklärung finden Sie auf unserer Website unter folgenden Link:"),
					new PdfTag("<%bodyTableFooterLink%>", $"https://www.psz-electronic.com/wp-content/uploads/2019/02/19-02-04-PSZ_Informationspflicht-Datenschutz-Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"),
					});

					// Footer
					string footer = HtmlPdfHelper.Template("PRS_ORD_Footer", new List<PdfTag> {
						new PdfTag("<%footerAddress1%>", "Im Gstaudach 6"),
					new PdfTag("<%footerAddress2%>", "92648 Vohenstrauß"),
					new PdfTag("<%footerAddress3%>", "Tel.: +49 9651 924 117-0"),

					new PdfTag("<%footerBankLabel%>", "Bankverbindung:"),
					new PdfTag("<%footerBankValue1%>", "Commerzbank AG Filiale Weiden"),
					new PdfTag("<%footerBankValue2%>", "Raiffeisenbank im Naabtal eG"),
					new PdfTag("<%footerBankValue3%>", "HypoVereinsbank Weiden"),
					new PdfTag("<%footerBankValue4%>", ""),

					new PdfTag("<%footerLabelUst%>", "Ust.-Id-Nr.:"),
					new PdfTag("<%footerValueUst%>", "DE 813 706 578"),
					new PdfTag("<%footerLabelSite%>", "Sitz:"),
					new PdfTag("<%footerValueSite%>", "Vohenstrauß"),
					new PdfTag("<%footerLabelFax%>", "Fax:"),
					new PdfTag("<%footerValueFax%>", "+49 9651 924 117-212"),


					new PdfTag("<%footerLabelManager%>", "Geschäftsführer:"),
					new PdfTag("<%footerValueManager%>", "Werner Steinbacher"),
					new PdfTag("<%footerLabelManager2%>", ""),
					new PdfTag("<%footerValueManager2%>", ""),
					new PdfTag("<%footerLabelTaxId%>", "Steuernummer:"),
					new PdfTag("<%footerValueTaxId%>", "255/135/40526"),

					new PdfTag("<%footerLabelHRB%>", "HRB:"),
					new PdfTag("<%footerValueHRB%>", "2907 AG Weiden"),
					new PdfTag("<%footerLabelEmail%>", "E-mail:"),
					new PdfTag("<%footerValueEmail%>", "info@psz-electronic.com"),
					new PdfTag("<%footerLabelCustomsId%>", "Zollnummer:"),
					new PdfTag("<%footerValueCustomsId%>", "488 26 28"),
					//Konto
					new PdfTag("<%footerAccountLabel%>", "Konto:"),
					new PdfTag("<%footerAccountValue1%>", "775 321 300"),
					new PdfTag("<%footerAccountValue2%>", "3 22 66 03"),
					new PdfTag("<%footerAccountValue3%>", "234 354 89"),
					new PdfTag("<%footerAccountValue4%>", ""),
					//BLZ
					new PdfTag("<%footerBLZLabel%>", "BLZ:"),
					new PdfTag("<%footerBLZValue1%>", "753 400 90"),
					new PdfTag("<%footerBLZValue2%>", "750691 71"),
					new PdfTag("<%footerBLZValue3%>", "753 200 75"),
					new PdfTag("<%footerBLZValue4%>", ""),
					//IBAN
					new PdfTag("<%footerIBANLabel%>", "IBAN:"),
					new PdfTag("<%footerIBANValue1%>", "DE41 7534 0090 0775 3213 00"),
					new PdfTag("<%footerIBANValue2%>", "DE04 7506 9171 0003 2266 03"),
					new PdfTag("<%footerIBANValue3%>", "DE56 7532 0075 0023 4354 89"),
					new PdfTag("<%footerIBANValue4%>", ""),
					//SWIFT
					new PdfTag("<%footerSWIFTLabel%>", "SWIFT-BIC:"),
					new PdfTag("<%footerSWIFTValue1%>", "COBADEFF753"),
					new PdfTag("<%footerSWIFTValue2%>", "GENODEF1SWD"),
					new PdfTag("<%footerSWIFTValue3%>", "HYVEDEMM454"),
					new PdfTag("<%footerSWIFTValue4%>", ""),

					//new PdfTag("<%foote%>", "value"),
					});
					return HtmlPdfHelper.Convert(body, header, footer, "");
				}
				#endregion
				return null;
			}
			public static byte[] GetKeineABVorhanden(List<ABNotAvailable> data)
			{

				var html = "<div style='font-size: 20px;padding-bottom:10px;padding-top:35px'><hr style='display: block;height: 1px;border:0;border-top:1px solid #ccc;margin 1em 0;padding:0;'> <b> Keine AB Vorhanden </b></div>";
				foreach(var item in data)
				{
					html += $@"<table  style='font-size: 16px;width:100%;padding-top:5px;'>
							  <tr>
								<th style='width:50%;text-align:left'><u>{item.Name1.Trim()}</u></th>
								<th style='width:25%;text-align:left'><u>Tel: {item.Telefon.Trim()}</u></th>
								<th style='width:25%;text-align:left'><u>Fax: {item.Fax.Trim()}</u></th>
							  </tr>
							</table>";
					foreach(var article in item.Items)
					{
						html += @$"<table style='font-size: 10px;width:100%;'><tr>";
						if(article.Datum.HasValue)
							html += $"<td style='width:10%'>{article.Datum.Value.ToString("dd.MM.yyyy")}</td>";
						else
							html += "<td style='width:10%'></td>";
						html += $@"
								<td  style='width:15%'>{article.BestellungNr}</td>
								<td style='width:5%'>{article.Lagerort_id} 	</td>
								<td style='width:5%'><b>{article.Anzahl} </b></td>
								<td style='width:20%'>{article.Artikelnummer} </td>
								<td style='width:35%'>{article.Bezeichnung_1} </td>";
						if(article.Liefertermin.HasValue)
							html += $"<td style='width:10%'>{article.Liefertermin.Value.ToString("dd.MM.yyyy")}</td>";
						else
							html += "<td style='width:10%'></td>";
						html += "</tr></table>";
					}
				}
				html += "<div></div";

				var footer = $"<div style='padding-left:25px'>{DateTime.Now.ToString("dddd, d. MMMM yyyy", new CultureInfo("de-DE"))}</div>";
				return HtmlPdfHelper.ConvertStatistics(html, null, footer, "");
			}
			public static byte[] GetAbTermin(List<DispositionDateDifference> data)
			{
				var html = "<div style='font-size: 20px;padding-bottom:5px;padding-top:35px'><hr style='display: block;height: 1px;border:0;border-top:1px solid #ccc;margin 1em 0;padding:0;'> <b> AB Termindiffernzen </b></div>";
				foreach(var item in data)
				{
					html += $@"<table style='font-size: 16px;width:100%;padding-top:10px;'>
							  <tr>
								<th style='width:50%;text-align:left'><u>{item.Name1.Trim()}</u></th>
								<th style='width:25%;text-align:left'><u>Tel: {item.Telefon.Trim()}</u></th>
								<th style='width:25%;text-align:left'><u>Fax: {item.Fax.Trim()}</u></th>
							  </tr>
							</table>";
					foreach(var article in item.Items)
					{
						html += @$"<table style='font-size: 10px;width:100%;'><tr>";
						if(article.Datum.HasValue)
							html += $"<td style='width:10%'>{article.Datum.Value.ToString("dd.MM.yyyy")}</td>";
						else
							html += "<td style='width:10%'></td>";
						html += $@"
								<td style='width:15%'>{article.BestellungNr}</td>
								<td style='width:5%'>{article.Lagerort_id} 	</td>
								<td style='width:5%'><b>{article.Anzahl} </b></td>
								<td style='width:15%'>{article.Artikelnummer} </td>
								<td style='width:30%'>{article.Bezeichnung_1} </td>";
						if(article.Liefertermin.HasValue)
							html += $"<td style='width:10%'>{article.Liefertermin.Value.ToString("dd.MM.yyyy")}</td>";
						else
							html += "<td style='width:10%'></td>";
						if(article.Bestatigter_Termin.HasValue)
							html += $"<td style='width:10%'><b>{article.Bestatigter_Termin.Value.ToString("dd.MM.yyyy")}</b></td>";
						else
							html += "<td style='width:10%'></td>";
						html += "</tr></table>";
					}
				}
				html += "<div></div";

				var footer = $"<div style='padding-left:25px'>{DateTime.Now.ToString("dddd, d. MMMM yyyy", new CultureInfo("de-DE"))}</div>";
				return HtmlPdfHelper.ConvertStatistics(html, null, footer, "");
			}
			#region models 
			public class OrderModel
			{
				public string Lieferanten_Nr { get; set; }
				public string Bestellung_Nr { get; set; }
				public string Typ { get; set; }
				public string Datum { get; set; }
				public string Anrede { get; set; }
				public string Vorname_NameFirma { get; set; }
				public string Name2 { get; set; }
				public string Name3 { get; set; }
				public string Strasse_Postfach { get; set; }
				public string Land_PLZ_Ort { get; set; }
				public string Personal_Nr { get; set; }
				public string Versandart { get; set; }
				public string Zahlungsweise { get; set; }
				public string Konditionen { get; set; }
				public string Zahlungsziel { get; set; }
				public string Bezug { get; set; }
				public string Ihr_Zeichen { get; set; }
				public string Unser_Zeichen { get; set; }
				public string Bestellbestatigung_erbeten_bis { get; set; }
				public string Freitext { get; set; }
				public string Wahrung { get; set; }
				public string Ansprechpartner { get; set; }
				public string Abteilung { get; set; }
				public string Nr { get; set; }
				public string Rahmenbestellung { get; set; }
				public string Mandant { get; set; }
				public string InfoRahmennummer { get; set; }
				public string RahmenBezug { get; set; }
				public string Langtexte { get; set; }

				public byte[] Logo { get; set; }
				public string Text_kopf { get; set; }
				public string Text_fuss { get; set; }

				public string Name { get; set; }
				public string Telefonnummer { get; set; }
				public string Faxnummer { get; set; }
				public string Email { get; set; }

				public string Telefon { get; set; }
				public string Fax { get; set; }

				public string Bestellung { get; set; }
				public string Bestellungen_unten { get; set; }
				public string Lager { get; set; }
				public string Symbol { get; set; }
				public List<PositionModel> PositionItems { get; set; }

				public class PositionModel
				{
					public string Bestellnummer { get; set; }
					public string Rabatt1 { get; set; }
					public string Rabatt2 { get; set; }
					public string Menge { get; set; }
					public string Einzelpreis { get; set; }
					public decimal Gesamtpreis { get; set; }
					public string Bezeichnung_1 { get; set; }
					public string Bezeichnung_2 { get; set; }
					public string Einheit { get; set; }
					public string Umsatzsteuer { get; set; }
					public string sortierung { get; set; }
					public string best_art_nr { get; set; }
					public string schrift { get; set; }
					public string Preiseinheit { get; set; }
					public string LT { get; set; }
					public string Lagerort_id { get; set; }
					public string CUPreis { get; set; }
					public string Artikelnummer { get; set; }
					public bool MHD { get; set; }
					public bool COF_Pflichtig { get; set; }
					public string Zeitraum_MHD { get; set; }
					public bool EMPB { get; set; }
					public bool ESD_Schutz { get; set; }
					public string Symbol { get; set; }
					public string LagerortLabel { get; set; }
					public string Position { get; set; }
					public decimal StartAnzahl { get; set; }
					public decimal Anzahl { get; set; }
					public decimal TotalPrice { get; set; }
					public string RahmenBezug { get; set; }
					public PositionModel(Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationReportEntity orderValidationReportEntity)
					{
						if(orderValidationReportEntity == null)
						{
							return;
						}
						Bestellnummer = orderValidationReportEntity.Bestellnummer;
						Rabatt1 = orderValidationReportEntity.Rabatt1;
						Rabatt2 = orderValidationReportEntity.Rabatt2;
						Menge = orderValidationReportEntity.Menge;
						Einzelpreis = orderValidationReportEntity.Einzelpreis;
						Gesamtpreis = orderValidationReportEntity.Gesamtpreis;
						Bezeichnung_1 = orderValidationReportEntity.Bezeichnung_1;
						Bezeichnung_2 = orderValidationReportEntity.Bezeichnung_2;
						Einheit = orderValidationReportEntity.Einheit;
						Umsatzsteuer = orderValidationReportEntity.Umsatzsteuer;
						sortierung = orderValidationReportEntity.sortierung;
						best_art_nr = orderValidationReportEntity.best_art_nr;
						schrift = orderValidationReportEntity.schrift;
						Preiseinheit = orderValidationReportEntity.Preiseinheit;
						LT = orderValidationReportEntity.LT;
						Lagerort_id = orderValidationReportEntity.Lagerort_id;
						CUPreis = orderValidationReportEntity.CUPreis;
						Artikelnummer = orderValidationReportEntity.Artikelnummer;
						MHD = orderValidationReportEntity.MHD;
						COF_Pflichtig = orderValidationReportEntity.COF_Pflichtig;
						Zeitraum_MHD = orderValidationReportEntity.Zeitraum_MHD;
						EMPB = orderValidationReportEntity.EMPB;
						ESD_Schutz = orderValidationReportEntity.ESD_Schutz;
						Symbol = orderValidationReportEntity.Symbol;
						LagerortLabel = orderValidationReportEntity.LagerortLabel;
						Position = orderValidationReportEntity.Position;
						Anzahl = orderValidationReportEntity.Anzahl;
						StartAnzahl = orderValidationReportEntity.StartAnzahl;
						RahmenBezug = orderValidationReportEntity.RahmenBezug;

					}
				}
				public OrderModel(Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationReportEntity orderValidationReportEntity)
				{
					if(orderValidationReportEntity == null)
					{
						return;
					}

					// - 
					Lieferanten_Nr = orderValidationReportEntity.Lieferanten_Nr;
					Bestellung_Nr = orderValidationReportEntity.Bestellung_Nr;
					Typ = orderValidationReportEntity.Typ;
					Datum = orderValidationReportEntity.Datum;
					Anrede = orderValidationReportEntity.Anrede;
					Vorname_NameFirma = orderValidationReportEntity.Vorname_NameFirma;
					Name2 = orderValidationReportEntity.Name2;
					Name3 = orderValidationReportEntity.Name3;
					Strasse_Postfach = orderValidationReportEntity.Strasse_Postfach;
					Land_PLZ_Ort = orderValidationReportEntity.Land_PLZ_Ort;
					Personal_Nr = orderValidationReportEntity.Personal_Nr;
					Versandart = orderValidationReportEntity.Versandart;
					Zahlungsweise = orderValidationReportEntity.Zahlungsweise;
					Konditionen = orderValidationReportEntity.Konditionen;
					Zahlungsziel = orderValidationReportEntity.Zahlungsziel;
					Bezug = orderValidationReportEntity.Bezug;
					Ihr_Zeichen = orderValidationReportEntity.Ihr_Zeichen;
					Unser_Zeichen = orderValidationReportEntity.Unser_Zeichen;
					Bestellbestatigung_erbeten_bis = orderValidationReportEntity.Bestellbestatigung_erbeten_bis;
					Freitext = orderValidationReportEntity.Freitext;
					Wahrung = orderValidationReportEntity.Wahrung;
					Ansprechpartner = orderValidationReportEntity.Ansprechpartner;
					Abteilung = orderValidationReportEntity.Abteilung;
					Nr = orderValidationReportEntity.Nr;
					Rahmenbestellung = orderValidationReportEntity.Rahmenbestellung;
					Mandant = orderValidationReportEntity.Mandant;
					InfoRahmennummer = orderValidationReportEntity.InfoRahmennummer;
					RahmenBezug = orderValidationReportEntity.RahmenBezug;
					Langtexte = orderValidationReportEntity.Langtexte;
					Text_kopf = orderValidationReportEntity.Text_kopf;
					Text_fuss = orderValidationReportEntity.Text_fuss;
					Telefon = orderValidationReportEntity.Telefon;
					Fax = orderValidationReportEntity.Fax;
					Name = orderValidationReportEntity.Name;
					Telefonnummer = orderValidationReportEntity.Telefonnummer;
					Faxnummer = orderValidationReportEntity.Faxnummer;
					Email = orderValidationReportEntity.Email;
					Bestellung = orderValidationReportEntity.Bestellung;
					Bestellungen_unten = orderValidationReportEntity.Bestellungen_unten;
					Lager = $"{orderValidationReportEntity.LagerortLabel}{orderValidationReportEntity.Bestellung_Nr}";
					Symbol = orderValidationReportEntity.Symbol;
					Logo = orderValidationReportEntity.Logo;
				}

			}
			public class ABNotAvailable
			{
				public string Name1 { get; set; }
				public string Telefon { get; set; }
				public string Fax { get; set; }
				public List<ReportItem> Items { get; set; }

				public List<ABNotAvailable> GetData(List<Infrastructure.Data.Entities.Views.MTM.View_PrioeinkaufEntity> view_PrioeinkaufEntity)
				{
					var response = new List<ABNotAvailable>();
					foreach(var item in view_PrioeinkaufEntity)
					{
						var supplier = item.Name1;
						if(response.Find(x => x.Name1.ToLower() == supplier.ToLower()) is null)
						{
							var supplierData = view_PrioeinkaufEntity.FindAll(x => x.Name1 == supplier);

							var ABNotAvailable = new ABNotAvailable();
							ABNotAvailable.Name1 = item.Name1;
							ABNotAvailable.Telefon = item.Telefon;
							ABNotAvailable.Fax = item.Fax;

							ABNotAvailable.Items = supplierData.Select(x => new ReportItem
							{
								Anzahl = x.Anzahl,
								Artikelnummer = x.Artikelnummer,
								BestellungNr = x.Bestellung_Nr,
								Bezeichnung_1 = x.Bezeichnung_1,
								Datum = x.Datum,
								Lagerort_id = x.Lagerort_id,
								Liefertermin = x.Liefertermin
							}).ToList();

							response.Add(ABNotAvailable);
						}
					}
					return response;
				}
			}
			public class DispositionDateDifference
			{
				public string Name1 { get; set; }
				public string Telefon { get; set; }
				public string Fax { get; set; }
				public List<ReortItemDateDifference> Items { get; set; }

				public List<DispositionDateDifference> GetData(List<Infrastructure.Data.Entities.Views.MTM.View_PSZ_Disposition_Ab_Termin_zu_Spat_sqlEntity> view_PSZ_Disposition_Ab_Termin_Zu_Spat_SqlEntities)
				{
					var response = new List<DispositionDateDifference>();
					foreach(var item in view_PSZ_Disposition_Ab_Termin_Zu_Spat_SqlEntities)
					{
						var supplier = item.Name1;
						if(response.Find(x => x.Name1.ToLower() == supplier.ToLower()) is null)
						{
							var supplierData = view_PSZ_Disposition_Ab_Termin_Zu_Spat_SqlEntities.FindAll(x => x.Name1 == supplier);

							var ABNotAvailable = new DispositionDateDifference();
							ABNotAvailable.Name1 = item.Name1;
							ABNotAvailable.Telefon = item.Telefon;
							ABNotAvailable.Fax = item.Fax;

							ABNotAvailable.Items = supplierData.Select(x => new ReortItemDateDifference
							{
								Anzahl = x.Anzahl,
								Artikelnummer = x.Artikelnummer,
								BestellungNr = x.Bestellung_Nr,
								Bezeichnung_1 = x.Bezeichnung_1,
								Datum = x.Datum,
								Lagerort_id = x.Lagerort_id,
								Liefertermin = x.Liefertermin,
								Bestatigter_Termin = x.Bestatigter_Termin
							}).ToList();

							response.Add(ABNotAvailable);
						}
					}
					return response;
				}

			}
			public class ReportItem
			{
				public DateTime? Datum { get; set; }
				public int? BestellungNr { get; set; }
				public int? Lagerort_id { get; set; }
				public decimal? Anzahl { get; set; }
				public string Artikelnummer { get; set; }
				public string Bezeichnung_1 { get; set; }
				public DateTime? Liefertermin { get; set; }
			}
			public class ReortItemDateDifference: ReportItem
			{
				public DateTime? Bestatigter_Termin { get; set; }
			}


			#endregion models
		}
		public static class CTS
		{
			public static byte[] GetPDF(DeliveryNoteModel deliveryNote, List<DeliveryNoteModel.DeliveryNoteItemModel> deliveryNoteItems)
			{
				#region convertHtml
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				string body = string.Empty;
				string invoiceHeader = string.Empty;
				string invoiceNumber = string.Empty;

				if(deliveryNote is not null)
				{
					string logo = "";
					var psz = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
					//REM:  - logo imported in STG should be PNG
					logo = $"data:image/png;base64,{Convert.ToBase64String(psz?.Logo)}";
					if(string.IsNullOrEmpty(logo))
					{
						// UNDONE : Fix logo
						logo = "data:image/png;base64,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";
					}
					//-
					string positionItems = string.Empty;
					positionItems += "";

					// - items
					var dataLS = deliveryNoteItems.OrderBy(x => x.ANgeboteArtikel_Liefertermin).Select(x => x.Angebote_Angebot_Nr ?? 0).Distinct().OrderBy(x => x);
					foreach(var itemData in dataLS)
					{
						// - Headers
						positionItems += $"<!-- br/><br/><br/><br/ --><span class='nobr'>&nbsp;&nbsp;LS-Nummer:</span> <b>{itemData}</b>";
						positionItems += $"<table class='pr-2' style='width: 100%;font-size:11px;table-layout:fixe-d;'>" +
							$"<tr><td colspan='6'><hr class='hr-2'></td></tr>";

						positionItems += $"<tr>" +
							$"<td><b>Artikelnummer</b></td>" +
							$"<td><div class='nobr'><b>Bezeichnung 1</b></div></td>" +
							$"<td><div class='nobr'><b>Bezeichnung 2</b></div></td>" +
							$"<td><b>Einheit</b></td>" +
							$"<td><b>Anzahl</b></td>" +
							$"<td><div class='nobr'><b>Stk-Gew</b>.</div><div class='nobr'><b>Ges-Gew</b>.</div></td>" +
						$"</tr>" +
						$"<tr><td colspan='6'><hr class='hr-2'></td></tr>";
						foreach(var item in deliveryNoteItems?.Where(x => x.Angebote_Angebot_Nr == itemData)?.OrderBy(x => x.Artikelnummer))
						{
							positionItems += $"<tr>" +
							$"<td >{item.Artikelnummer}<br/><span class='nobr'>Ihre Bestellung/PO#:</span> <b>{item.Angebote_Bezug}</b></td>" +
							$"<td>{item.ANgeboteArtikel_Bezeichnung1}</td>" +
							$"<td>{item.ANgeboteArtikel_Bezeichnung2}<br/>Lieferdatum:<b>{item.ANgeboteArtikel_Liefertermin?.ToString("dd/MM/yyyy")}</b></td>" +
							$"<td class='nobr'>{item.ANgeboteArtikel_Einheit}<br/>Ursprungsland:<b>{item.Ursprungsland}</b></td>" +
							$"<td class='nobr'>{item.ANgeboteArtikel_Anzahl}<br/>Zolltarif-Nr.:<b>{item.Zolltarif_nr}</b></td>" +
							$"<td class='nobr' align='right'>{$"{item.ANgeboteArtikel_EinzelCu_Gewicht}".FormatDecimal(2)}kg<br/>{$"{item.ANgeboteArtikel_GesamtCu_Gewicht}".FormatDecimal(2)}kg</td>" +
							$"</tr>" +
							$"<tr><td colspan='6'><hr class='hr'></td></tr>";
						}

						positionItems += $"</table>";
					}

					positionItems += $"<table class='pr-2' style='width: 100%;font-size:11px;table-layout:fixe-d;'><tr><td align='right'>Gewicht: <b>{$"{deliveryNoteItems.Select(x => x.ANgeboteArtikel_GesamtCu_Gewicht ?? 0).Sum()}".FormatDecimal(2)}</b>kg</td></tr></table>";


					body = HtmlPdfHelper.Template("CTS_STK_DNzuSTLG_Body", new List<PdfTag> {
					new PdfTag("<%bodyLogo%>", logo),

					new PdfTag("<%bodyPSZAddress%>", "PSZ electronic GmbH • Im Gstaudach 6 • 92648 Vohenstrauß"),
					new PdfTag("<%bodyLAnrede%>", content: $"{deliveryNote.LAnrede}"),
					new PdfTag("<%bodyLName1%>", content: $"{deliveryNote.LName1}"),
					new PdfTag("<%bodyLName2%>", content: $"{deliveryNote.LName2}"),
					new PdfTag("<%bodyLStreet%>", content: $"{deliveryNote.LStreet}"),
					new PdfTag("<%bodyLCountry%>", content: $"{deliveryNote.LCountry}"),
					new PdfTag("<%bodyDocumentTitle%>", content: $"{deliveryNote.DocumentTitle}"),
					new PdfTag("<%bodyMessageHeader%>", content: $"{deliveryNote.MessageHeader}"),

					new PdfTag("<%bodyCustomerNumberLabel%>", "Ihr Zeichen:"),
					new PdfTag("<%bodyCustomerNumberValue%>", content: $"{deliveryNote.CustomerNumber}"),
					new PdfTag("<%bodyShippingMethodLabel%>", "Versandart:"),
					new PdfTag("<%bodyShippingMethodValue%>", $"{deliveryNote.ShippingMethod}"),
					new PdfTag("<%bodyVAT_ID%>", $"{deliveryNote.VAT_ID}"),
					new PdfTag("<%bodyPosText%>", $"{deliveryNote.PosText}"),


					new PdfTag("<%positionItems%>", $"{positionItems}"),

					new PdfTag("<%bodyRGAddress%>", content: $"Rechnungsadresse"),
					new PdfTag("<%bodyAnrede%>", content: $"{deliveryNote.Anrede}"),
					new PdfTag("<%bodyName1%>", content: $"{deliveryNote.Name1}"),
					new PdfTag("<%bodyName2%>", content: $"{deliveryNote.Name2}"),
					new PdfTag("<%bodyStreet%>", content: $"{deliveryNote.Street}"),
					new PdfTag("<%bodyCountry%>", content: $"{deliveryNote.Country}"),
					});

					// Footer
					string footer = HtmlPdfHelper.Template("CTS_STK_DNzuSTLG_Footer", new List<PdfTag> {
						new PdfTag("<%footerAddress1%>", "Im Gstaudach 6"),
					new PdfTag("<%footerAddress2%>", "92648 Vohenstrauß"),
					new PdfTag("<%footerAddress3%>", "Tel.: +49 9651 924 117-0"),

					new PdfTag("<%footerBankLabel%>", "Bankverbindung:"),
					new PdfTag("<%footerBankValue1%>", "Commerzbank AG Filiale Weiden"),
					new PdfTag("<%footerBankValue2%>", "Raiffeisenbank im Naabtal eG"),
					new PdfTag("<%footerBankValue3%>", "HypoVereinsbank Weiden"),
					new PdfTag("<%footerBankValue4%>", ""),

					new PdfTag("<%footerLabelUst%>", "Ust.-Id-Nr.:"),
					new PdfTag("<%footerValueUst%>", "DE 813 706 578"),
					new PdfTag("<%footerLabelSite%>", "Sitz:"),
					new PdfTag("<%footerValueSite%>", "Vohenstrauß"),
					new PdfTag("<%footerLabelFax%>", "Fax:"),
					new PdfTag("<%footerValueFax%>", "+49 9651 924 117-212"),


					new PdfTag("<%footerLabelManager%>", "Geschäftsführer:"),
					new PdfTag("<%footerValueManager%>", "Werner Steinbacher"),
					new PdfTag("<%footerLabelManager2%>", ""),
					new PdfTag("<%footerValueManager2%>", ""),
					new PdfTag("<%footerLabelTaxId%>", "Steuernummer:"),
					new PdfTag("<%footerValueTaxId%>", "255/135/40526"),

					new PdfTag("<%footerLabelHRB%>", "HRB:"),
					new PdfTag("<%footerValueHRB%>", "2907 AG Weiden"),
					new PdfTag("<%footerLabelEmail%>", "E-mail:"),
					new PdfTag("<%footerValueEmail%>", "info@psz-electronic.com"),
					new PdfTag("<%footerLabelCustomsId%>", "Zollnummer:"),
					new PdfTag("<%footerValueCustomsId%>", "488 26 28"),
					//Konto
					new PdfTag("<%footerAccountLabel%>", "Konto:"),
					new PdfTag("<%footerAccountValue1%>", "775 321 300"),
					new PdfTag("<%footerAccountValue2%>", "3 22 66 03"),
					new PdfTag("<%footerAccountValue3%>", "234 354 89"),
					new PdfTag("<%footerAccountValue4%>", ""),
					//BLZ
					new PdfTag("<%footerBLZLabel%>", "BLZ:"),
					new PdfTag("<%footerBLZValue1%>", "753 400 90"),
					new PdfTag("<%footerBLZValue2%>", "750691 71"),
					new PdfTag("<%footerBLZValue3%>", "753 200 75"),
					new PdfTag("<%footerBLZValue4%>", ""),
					//IBAN
					new PdfTag("<%footerIBANLabel%>", "IBAN:"),
					new PdfTag("<%footerIBANValue1%>", "DE41 7534 0090 0775 3213 00"),
					new PdfTag("<%footerIBANValue2%>", "DE04 7506 9171 0003 2266 03"),
					new PdfTag("<%footerIBANValue3%>", "DE56 7532 0075 0023 4354 89"),
					new PdfTag("<%footerIBANValue4%>", ""),
					//SWIFT
					new PdfTag("<%footerSWIFTLabel%>", "SWIFT-BIC:"),
					new PdfTag("<%footerSWIFTValue1%>", "COBADEFF753"),
					new PdfTag("<%footerSWIFTValue2%>", "GENODEF1SWD"),
					new PdfTag("<%footerSWIFTValue3%>", "HYVEDEMM454"),
					new PdfTag("<%footerSWIFTValue4%>", ""),

					//new PdfTag("<%foote%>", "value"),
					});
					return HtmlPdfHelper.Convert(body, header, footer, "", logo);
				}
				#endregion

				// - 
				return null;
			}
			public static async Task<byte[]> GetRahmen(RahmenModel rahmenData, DocFooterModel footerData)
			{
				return await GetRahmenReportAsync(rahmenData, footerData);
			}
			public static async Task<byte[]> GetAB(CTS_ABReportModel abData, DocFooterModel footerData)
			{
				return await GetABReportAsync(abData, footerData);
			}
			public static async Task<byte[]> GetLS(CTS_LSReportModel lsData, DocFooterModel footerData)
			{
				return await GetLSReportAsync(lsData, footerData);
			}
			public static async Task<byte[]> GetGS(CTS_GSReportModel gsData, DocFooterModel footerData)
			{
				return await GetGSReportAsync(gsData, footerData);
			}
			public static async Task<byte[]> GetCTSRG(RechnungModel ctsRgData, CTS_StatsRG_DocFooterModel footerData)
			{
				return await GetCTSStatsRechnungReportAsync(ctsRgData, footerData);
			}
			public static async Task<byte[]> GetRahmenReportAsync(RahmenModel rahmenData, DocFooterModel footerData)
			{
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				if(rahmenData is not null)
				{
					return await HtmlPdfHelper.GetRahmenReport(rahmenData, footerData, header, $" Rahmen {rahmenData.Nr}");
				}
				return null;
			}
			public static async Task<byte[]> GetABReportAsync(CTS_ABReportModel abData, DocFooterModel footerData)
			{
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				if(abData is not null)
				{
					return await HtmlPdfHelper.GetABReport(abData, footerData, header, $" von {abData.DocumentType} {abData.OrderNumberValue}");
				}
				return null;
			}
			public static async Task<byte[]> GetLSReportAsync(CTS_LSReportModel lsData, DocFooterModel footerData)
			{
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				if(lsData is not null)
				{
					return await HtmlPdfHelper.GetLSReport(lsData, footerData, header, $" von {lsData.DocumentType} {lsData.OrderNumberValue}");
				}
				return null;
			}
			public static async Task<byte[]> GetGSReportAsync(CTS_GSReportModel gsData, DocFooterModel footerData)
			{
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				if(gsData is not null)
				{
					return await HtmlPdfHelper.GetGSReport(gsData, footerData, header, $" von {gsData.DocumentType} {gsData.OrderNumberValue}");
				}
				return null;
			}
			public static async Task<byte[]> GetCTSStatsRechnungReportAsync(RechnungModel ctsRgData, CTS_StatsRG_DocFooterModel footerData)
			{
				string header = HtmlPdfHelper.Template("PRS_ORD_Header");
				if(ctsRgData is not null)
				{
					return await HtmlPdfHelper.GetCTSRechnungReport(ctsRgData, footerData, header);
				}
				return null;
			}
		}
		#region models
		public record PdfTag
		{
			public string tag { get; set; }
			public string content { get; set; }

			public PdfTag(string tag, string content)
			{
				this.tag = tag;
				this.content = content;
			}
		}

		public class HtmlPdfHelper
		{
			const int PLACEHOLDER_SIZE = 8;
			private class FooterEventHandler: IEventHandler
			{
				public string html;
				public int height = 130;
				public FooterEventHandler(string html)
				{
					this.html = html;
				}
				public FooterEventHandler(string html, int height)
				{
					this.height = height;
					this.html = html;
				}
				public void HandleEvent(Event currentEvent)
				{
					PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
					iText.Kernel.Geom.Rectangle pageSize = docEvent.GetPage().GetPageSize();
					var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(docEvent.GetPage());
					Canvas c = new iText.Layout.Canvas(canvas, new iText.Kernel.Geom.Rectangle(0, 0, pageSize.GetWidth(), 130));
					PdfDocument pdf = docEvent.GetDocument();
					int numberOfPages = pdf.GetNumberOfPages();

					foreach(var item in HtmlConverter.ConvertToElements(html))
					{
						c.Add((IBlockElement)item);
					}
					c.Flush();
					c.Close();

				}
			}
			private class HeaderEventHandler: IEventHandler
			{
				protected PdfFormXObject placeholder;
				protected PdfFormXObject placeholderForFirstPage;
				string complement = string.Empty;
				public Boolean isSpecialFirstPage = true;
				public string html;
				protected float side = 20;
				protected float x = 200;
				protected float firstPagePageNumberPosition = 30;
				protected float yHeader = 720;
				protected float y = 800;
				protected float space = 55f;
				protected float descent = 3;
				protected string image = "";
				protected float offsetYPageNumber = 0;
				protected bool setPages;
				public HeaderEventHandler(string html, string complement, string image = null, bool setPages = true)
				{
					this.complement = complement;
					this.html = html;
					this.placeholder = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
					this.placeholderForFirstPage = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
					this.image = image;
					if(string.IsNullOrWhiteSpace(this.image))
					{
						this.image = System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo);
					}

					this.setPages = setPages;
				}
				public HeaderEventHandler(string html, string complement, Boolean isSpecialFirstPage, float offsetYPageNumber)
				{
					this.complement = complement;
					this.isSpecialFirstPage = isSpecialFirstPage;
					this.offsetYPageNumber = offsetYPageNumber;
					this.html = html;
					this.placeholder = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
					this.placeholderForFirstPage = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));

				}
				public void WriteTotal(PdfDocument pdf)
				{
					Canvas canvas = new Canvas(placeholder, pdf);
					// Set font, font size, font color, and other text properties
					canvas.SetFontSize(PLACEHOLDER_SIZE)
						.SetFontColor(ColorConstants.LIGHT_GRAY)
						.SetTextAlignment(TextAlignment.CENTER).SetItalic();
					//x=20
					canvas.ShowTextAligned(pdf.GetNumberOfPages() + complement.ToString(), 20, descent, TextAlignment.LEFT);
					canvas.Close();

					Canvas canvasC2 = new Canvas(placeholderForFirstPage, pdf);
					canvasC2.SetFontSize(PLACEHOLDER_SIZE).SetFontColor(ColorConstants.BLACK).SetItalic();
					canvasC2.ShowTextAligned(pdf.GetNumberOfPages().ToString(), 0, descent, TextAlignment.LEFT);
					canvasC2.Close();


				}
				public void HandleEvent(Event @currentEvent)
				{

					PdfDocumentEvent docEvent = (PdfDocumentEvent)@currentEvent;
					PdfDocument pdf = docEvent.GetDocument();
					iText.Kernel.Pdf.PdfPage page = docEvent.GetPage();
					int pageNumber = pdf.GetPageNumber(page);
					// do not add header in the first page
					//if(pageNumber>1)
					Canvas c = null;
					Text t = null;
					PdfCanvas canvas = null;
					iText.Kernel.Geom.Rectangle pageSize = docEvent.GetPage().GetPageSize();
					canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(docEvent.GetPage());
					if(setPages)
					{
						if(pageNumber == 1)
						{

							c = new iText.Layout.Canvas(canvas, new Rectangle(0, 0, pageSize.GetWidth(), 40));
							t = new Text("Seite " + pageNumber.ToString() + " von");
							Paragraph p = new Paragraph().Add(t);
							p.SetFontColor(ColorConstants.BLACK);
							p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
							c.ShowTextAligned(p, x + 330, yHeader, TextAlignment.RIGHT);
							canvas.AddXObjectAt(placeholderForFirstPage, x + 335, yHeader - descent);
							canvas.Release();
						}
						else
						{
							float imageWidth = 60;
							float imageHeight = 60;
							float imageX = pageSize.GetWidth() - imageWidth - x;
							x = 50;
							c = new iText.Layout.Canvas(canvas, new Rectangle(0, 0, pageSize.GetWidth(), 100));
							t = new Text("Seite/Page " + pageNumber.ToString() + " von/of ");
							Paragraph p = new Paragraph().Add(t);
							p.SetFontColor(ColorConstants.LIGHT_GRAY);
							p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
							p.SetWidth(pageSize.GetWidth() - 80);
							p.SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f));

							if(!string.IsNullOrWhiteSpace(image))
							{
								var imageBytes = System.Convert.FromBase64String(image.Replace("data:image/png;base64,", ""));
								Image img = new Image(ImageDataFactory.Create(imageBytes));
								img.SetAutoScale(false);
								img.ScaleToFit(imageWidth, imageHeight);
								img.SetRelativePosition(380, 1, 10, 100);
								p.Add(img);
							}

							c.ShowTextAligned(p, x, y, TextAlignment.LEFT);
							canvas.AddXObjectAt(placeholder, x + space, y - descent);
							canvas.Release();
						}
					}
					// Create placeholder object to write number of pages
					canvas.Release();
					if(setPages)
					{
						c.Flush();
						c.Close();
					}
				}
			}
			public static byte[] ConvertStatistics(string htmlToConvert, string htmlHeader = null, string htmlFooter = null, string docName = "")
			{
				using(MemoryStream stream = new MemoryStream())
				{
					using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(stream))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, false, 10f);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter, 30));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();


						HtmlConverter.ConvertToDocument(htmlToConvert, pdfDocument, converterProperties);
						// Write the total number of pages to the placeholder
						headerHandler.WriteTotal(pdfDocument);

						pdfDocument.Close();
						return stream.ToArray();
					}
				}


			}
			public static byte[] Convert(string htmlToConvert, string htmlHeader = null, string htmlFooter = null, string docName = "", string logo = null)
			{
				using(MemoryStream steam = new MemoryStream())
				{
					using(var writer = new iText.Kernel.Pdf.PdfWriter(steam))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, logo);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();


						HtmlConverter.ConvertToDocument(htmlToConvert, pdfDocument, converterProperties);
						// Write the total number of pages to the placeholder
						headerHandler.WriteTotal(pdfDocument);
						pdfDocument.Close();
						return steam.ToArray();
					}
				}

			}
			public static async Task<byte[]> GetRahmenReport(RahmenModel rahmenData, DocFooterModel footerData, string htmlHeader = null, string docName = "")
			{
				rahmenData.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				//body
				string bodyTemplatePath = @$"Reporting\Templates\IText\CTS_RA_Body.cshtml";
				string bodyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bodyTemplatePath);
				var bodyEngine = new RazorLightEngineBuilder()
				.UseFileSystemProject(System.IO.Path.GetDirectoryName(bodyPath))
				.UseMemoryCachingProvider()
				.Build();
				var htmlBody = await bodyEngine.CompileRenderAsync(System.IO.Path.GetFileName(bodyTemplatePath), rahmenData);
				//footer
				var footerTemplatePath = @"Reporting\Templates\IText\Footer.cshtml";
				string footerPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), footerTemplatePath);
				var footerEngine = new RazorLightEngineBuilder()
			   .UseFileSystemProject(System.IO.Path.GetDirectoryName(footerPath))
			   .UseMemoryCachingProvider()
			   .Build();
				var htmlFooter = await footerEngine.CompileRenderAsync(System.IO.Path.GetFileName(footerTemplatePath), footerData);
				using(var memoryStream = new MemoryStream())
				{
					using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, rahmenData.Logo);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();
						HtmlConverter.ConvertToDocument(htmlBody, pdfDocument, converterProperties);
						headerHandler.WriteTotal(pdfDocument);
						pdfDocument.Close();
						return memoryStream.ToArray();
					}
				}
			}
			public static async Task<byte[]> GetABReport(CTS_ABReportModel abData, DocFooterModel footerData, string htmlHeader = null, string docName = "")
			{
				abData.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				//body
				string bodyTemplatePath = @$"Reporting\Templates\IText\CTS_AB_Body.cshtml";
				string bodyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bodyTemplatePath);
				var bodyEngine = new RazorLightEngineBuilder()
				.UseFileSystemProject(System.IO.Path.GetDirectoryName(bodyPath))
				.UseMemoryCachingProvider()
				.Build();
				var htmlBody = await bodyEngine.CompileRenderAsync(System.IO.Path.GetFileName(bodyTemplatePath), abData);
				//footer
				var footerTemplatePath = @"Reporting\Templates\IText\Footer.cshtml";
				string footerPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), footerTemplatePath);
				var footerEngine = new RazorLightEngineBuilder()
			   .UseFileSystemProject(System.IO.Path.GetDirectoryName(footerPath))
			   .UseMemoryCachingProvider()
			   .Build();
				var htmlFooter = await footerEngine.CompileRenderAsync(System.IO.Path.GetFileName(footerTemplatePath), footerData);
				using(var memoryStream = new MemoryStream())
				{
					using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, abData.Logo);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();
						HtmlConverter.ConvertToDocument(htmlBody, pdfDocument, converterProperties);
						headerHandler.WriteTotal(pdfDocument);
						pdfDocument.Close();
						return memoryStream.ToArray();
					}
				}
			}
			public static async Task<byte[]> GetLSReport(CTS_LSReportModel lsData, DocFooterModel footerData, string htmlHeader = null, string docName = "")
			{
				lsData.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				//body
				string bodyTemplatePath = @$"Reporting\Templates\IText\CTS_LS_Body.cshtml";
				string bodyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bodyTemplatePath);
				var bodyEngine = new RazorLightEngineBuilder()
				.UseFileSystemProject(System.IO.Path.GetDirectoryName(bodyPath))
				.UseMemoryCachingProvider()
				.Build();
				var htmlBody = await bodyEngine.CompileRenderAsync(System.IO.Path.GetFileName(bodyTemplatePath), lsData);
				//footer
				var footerTemplatePath = @"Reporting\Templates\IText\Footer.cshtml";
				string footerPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), footerTemplatePath);
				var footerEngine = new RazorLightEngineBuilder()
			   .UseFileSystemProject(System.IO.Path.GetDirectoryName(footerPath))
			   .UseMemoryCachingProvider()
			   .Build();
				var htmlFooter = await footerEngine.CompileRenderAsync(System.IO.Path.GetFileName(footerTemplatePath), footerData);
				using(var memoryStream = new MemoryStream())
				{
					using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, lsData.Logo);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();
						HtmlConverter.ConvertToDocument(htmlBody, pdfDocument, converterProperties);
						headerHandler.WriteTotal(pdfDocument);
						pdfDocument.Close();
						return memoryStream.ToArray();
					}
				}
			}
			public static async Task<byte[]> GetGSReport(CTS_GSReportModel gsData, DocFooterModel footerData, string htmlHeader = null, string docName = "")
			{
				gsData.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				//body
				string bodyTemplatePath = @$"Reporting\Templates\IText\CTS_GS_Body.cshtml";
				string bodyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bodyTemplatePath);
				var bodyEngine = new RazorLightEngineBuilder()
				.UseFileSystemProject(System.IO.Path.GetDirectoryName(bodyPath))
				.UseMemoryCachingProvider()
				.Build();
				var htmlBody = await bodyEngine.CompileRenderAsync(System.IO.Path.GetFileName(bodyTemplatePath), gsData);
				//footer
				var footerTemplatePath = @"Reporting\Templates\IText\Footer.cshtml";
				string footerPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), footerTemplatePath);
				var footerEngine = new RazorLightEngineBuilder()
			   .UseFileSystemProject(System.IO.Path.GetDirectoryName(footerPath))
			   .UseMemoryCachingProvider()
			   .Build();
				var htmlFooter = await footerEngine.CompileRenderAsync(System.IO.Path.GetFileName(footerTemplatePath), footerData);
				using(var memoryStream = new MemoryStream())
				{
					using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, gsData.Logo);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();
						HtmlConverter.ConvertToDocument(htmlBody, pdfDocument, converterProperties);
						headerHandler.WriteTotal(pdfDocument);
						pdfDocument.Close();
						return memoryStream.ToArray();
					}
				}
			}
			public static async Task<byte[]> GetCTSRechnungReport(RechnungModel ctsRgData, CTS_StatsRG_DocFooterModel footerData, string htmlHeader = null, string docName = "")
			{
				//ctsRgData.ReportParameters.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				//legend image
				//string imagePath = @$"Reporting\Templates\IText\legend.png";
				//string imgaeFullPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), imagePath);
				//byte[] imageBytes = File.ReadAllBytes(imgaeFullPath);
				//string base64String = System.Convert.ToBase64String(imageBytes);
				//ctsRgData.ReportParameters.Legend = $"data:image/png;base64,{base64String}";
				//body
				string bodyTemplatePath = @$"Reporting\Templates\IText\CTS_STATS_RG_Body.cshtml";
				string bodyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bodyTemplatePath);
				var bodyEngine = new RazorLightEngineBuilder()
				.UseFileSystemProject(System.IO.Path.GetDirectoryName(bodyPath))
				.UseMemoryCachingProvider()
				.Build();
				var htmlBody = await bodyEngine.CompileRenderAsync(System.IO.Path.GetFileName(bodyTemplatePath), ctsRgData);
				//footer
				var footerTemplatePath = @"Reporting\Templates\IText\CTS_STATS_RG_Footer.cshtml";
				string footerPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), footerTemplatePath);
				var footerEngine = new RazorLightEngineBuilder()
			   .UseFileSystemProject(System.IO.Path.GetDirectoryName(footerPath))
			   .UseMemoryCachingProvider()
			   .Build();
				var htmlFooter = await footerEngine.CompileRenderAsync(System.IO.Path.GetFileName(footerTemplatePath), footerData);
				using(var memoryStream = new MemoryStream())
				{
					using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream))
					{
						PdfDocument pdfDocument = new PdfDocument(writer);
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName, ctsRgData.ReportParameters.Logo, false);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();
						HtmlConverter.ConvertToDocument(htmlBody, pdfDocument, converterProperties);
						headerHandler.WriteTotal(pdfDocument);
						pdfDocument.Close();
						return memoryStream.ToArray();
					}
				}
			}
			public static string Template(string template, List<PdfTag> lsTags = null)
			{
				if(!string.IsNullOrEmpty(template))
				{
					string templatePath = @$"Reporting\Templates\IText\{template}.html";
					string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), templatePath);
					if(File.Exists(path))
					{
						template = File.ReadAllText(path);
						if(lsTags is not null)
						{
							foreach(PdfTag pdftag in lsTags)
							{
								template = template.Replace(pdftag.tag, pdftag.content);
							}
						}
					}

				}
				return template;
			}
			/// <summary>
			/// Template is deprecated
			/// We should use LoadTemplate which will load template within a given module 
			/// Otherwise the number templates will be enormes we should put every template in the correct module
			/// </summary>
			/// <param name="module"></param>
			/// <param name="template"></param>
			/// <param name="lsTags"></param>
			/// <returns></returns>
			public static string LoadTemplate(string module, string template, List<PdfTag> lsTags = null)
			{
				if(!string.IsNullOrEmpty(module) && !string.IsNullOrEmpty(template))
				{
					string templatePath = @$"Reporting\Templates\IText\{module}\{template}.html";
					string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), templatePath);
					if(File.Exists(path))
					{
						template = File.ReadAllText(path);
						if(lsTags is not null)
						{
							foreach(PdfTag pdftag in lsTags)
							{
								template = template.Replace(pdftag.tag, pdftag.content);
							}
						}
					}

				}
				return template;
			}
		}
		#endregion models
	}
}
