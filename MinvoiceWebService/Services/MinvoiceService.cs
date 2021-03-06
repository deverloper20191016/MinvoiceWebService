﻿using MinvoiceWebService.Converts;
using MinvoiceWebService.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MinvoiceWebService.Services
{
	public class MinvoiceService
	{
		private static DataRequestObject SetupDataRequestObject(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int? signType, int typeOfInvoice = 1, int typeUpdate = 1)
		{
			// test commit 1
			DataRequestObject dataRequestObject = new DataRequestObject
			{
				KyHieu = kyHieu,
				Opt = opt,
				InvoiceNumber = invoiceNumber,
				TypeOfInvoice = typeOfInvoice,
				MauSo = mauSo,
				XmlData = xml,
				Mst = mst,
				Password = passWord,
				Username = userName,
				TypeUpdate = typeUpdate,
				SignType = signType,
				InvInvoiceCodeId = ApiService.GetInvInvoiceCodeId(mst, userName, passWord, mauSo, kyHieu)
			};


			return dataRequestObject;
		}

		public static string GetInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu,
			string invoiceNumber)
		{
			JObject json = new JObject();
			try
			{
				JArray jArrayInvoice = ApiService.GetInvoice(userName, passWord, kyHieu, mauSo, invoiceNumber, mst);
				if (jArrayInvoice.Count > 0)
				{
					JToken jObject = jArrayInvoice[0];
					json.Add("id", jObject["inv_InvoiceAuth_id"]);
					json.Add("inv_invoiceNumber", jObject["inv_invoiceNumber"]);
					json.Add("key", jObject["so_benh_an"]);
					json.Add("trang_thai", jObject["trang_thai"]);
					json.Add("trang_thai_hd", jObject["trang_thai_hd"]);
					json.Add("mau_so", jObject["mau_hd"]);
					json.Add("ky_hieu", jObject["inv_invoiceSeries"]);



					int trangThaiKy = jObject["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : jObject["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;

					if (trangThaiKy != 1)
					{
						json.Add("sobaomat", jObject["sobaomat"]);
					}

					JObject result = new JObject
					{
						{$"OK_{invoiceNumber};{jObject["trang_thai_hd"]}_{trangThaiKy}", json}
					};
					return result.ToString();
				}
				else
				{
					json.Add($"ERROR_{invoiceNumber}", $"Không tìm thấy hóa đơn có số {invoiceNumber}, mẫu số {mauSo}, ký hiệu {kyHieu}");
					return json.ToString();
				}
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mst"></param>
		/// <param name="userName"></param>
		/// <param name="passWord"></param>
		/// <param name="mauSo"></param>
		/// <param name="kyHieu"></param>
		/// <param name="invoiceNumber"></param>
		/// <param name="xml"></param>
		/// <param name="opt"></param>
		/// <param name="typeOfInvoice">Trường inv_adjustmentType của hóa đơn</param>
		/// <param name="typeUpdate">Loại update. 2: Điều chỉnh, 1: Cập nhật hoặc thêm mới</param>
		/// <param name="typeOfAdjustment">Loại điều chỉnh. 1: Tăng, 2: Giảm, 3: Định danh, 4: Thay thế, 5: Tăng/Giảm</param>
		/// <returns></returns>
		public static string UpdateInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int typeOfInvoice = 1, int typeUpdate = 1, int typeOfAdjustment = 1)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}
			if (opt && string.IsNullOrEmpty(invoiceNumber))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập số hóa đơn muốn điều chỉnh hoặc cập nhật");
				return jObjectResult.ToString();
			}
			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, opt, null, typeOfInvoice, typeUpdate);

			JArray jArrayInvoice = ApiService.GetInvoice(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, dataRequestObject.InvoiceNumber, mst);

			if (dataRequestObject.TypeUpdate == 2 || dataRequestObject.Opt)
			{

				if (jArrayInvoice.Count > 0)
				{
					// Nếu chọn điều chỉnh tạo hóa đơn mới điều chỉnh cho hóa đơn gốc
					if (dataRequestObject.TypeUpdate == 2)
					{
						if (jArrayInvoice[0]["trang_thai"].ToString() != "Đã ký")
						{
							jObjectResult.Add($"ERROR_{dataRequestObject.InvoiceNumber}", $"Hóa đơn số {dataRequestObject.InvoiceNumber} chưa ký không thể điều chỉnh.");
							return jObjectResult.ToString();
						}

						// Lấy Id của hóa đơn gốc
						dataRequestObject.InvOriginalId = jArrayInvoice[0]["inv_InvoiceAuth_id"].ToString();
						// Gán trạng thái cho hóa đơn điều chỉnh
						dataRequestObject.TypeOfInvoice = typeOfAdjustment == 1 ? 19 : typeOfAdjustment == 2 ? 21 : typeOfAdjustment == 3 ? 5 : typeOfAdjustment == 4 ? 3 : 23;
						dataRequestObject.TypeOfInvoiceParent = typeOfAdjustment == 1 ? 11 :
							typeOfAdjustment == 2 ? 11 :
							typeOfAdjustment == 3 ? 11 : typeOfAdjustment == 4 ? 17 : 11;
					}
					else
					{
						if (jArrayInvoice[0]["trang_thai"].ToString().Equals("Đã ký"))
						{
							jObjectResult.Add($"ERROR_{dataRequestObject.InvoiceNumber}", $"Hóa đơn số {dataRequestObject.InvoiceNumber} đã ký không thể sửa.");
							return jObjectResult.ToString();
						}

						dataRequestObject.InvInvoiceCodeId = jArrayInvoice[0]["inv_InvoiceCode_id"].ToString();
					}
				}
				else
				{
					jObjectResult.Add($"ERROR_{dataRequestObject.InvoiceNumber}", $"Không tồn tại số hóa đơn số: {dataRequestObject.InvoiceNumber} Mẫu số: {dataRequestObject.MauSo} Ký hiệu: {dataRequestObject.KyHieu} ! ");
					return jObjectResult.ToString();
				}
			}

			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{


						if (dataRequestObject.TypeUpdate == 2)
						{
							if (string.IsNullOrEmpty(invoice.Master.NumberOrDucument) || string.IsNullOrEmpty(invoice.Master.DateOrDucument) || string.IsNullOrEmpty(invoice.Master.NoteOfDocument))
							{
								jObjectResult.Add($"ERROR_{dataRequestObject.InvoiceNumber}", "Vui lòng nhập đủ thông tin điều chỉnh");
								return jObjectResult.ToString();
							}
							JArray jArrayInvoiceKey = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
							if (jArrayInvoiceKey.Count > 0)
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
								return jObjectResult.ToString();
							}

						}
						JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
						string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlAddApi}";
						string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
						WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
						string rs = webClient.UploadString(url, dataRequest);
						JObject dataResponse = JObject.Parse(rs);

						if (dataResponse.ContainsKey("error"))
						{
							jObjectResult.Add($"ERROR_{dataRequestObject.InvoiceNumber}", $"{dataResponse["error"]}");
						}
						else
						{
							if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
							{
								int trangThaiKy = jArrayInvoice[0]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : jArrayInvoice[0]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
								int trangThaiKyNew = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
								jObjectResult.Add($"OK_{dataRequestObject.InvoiceNumber};{jArrayInvoice[0]["trang_thai_hd"]}_{trangThaiKy}", (dataRequestObject.TypeUpdate == 2 ? "Điều chỉnh" : "Cập nhật") + ($" hóa đơn số {dataRequestObject.InvoiceNumber} thành công. "));
								if (typeUpdate == 2)
								{

									string sqlUpdate =
										$"UPDATE dbo.inv_InvoiceAuth SET trang_thai_hd = {dataRequestObject.TypeOfInvoiceParent}, inv_adjustmentType =  {dataRequestObject.TypeOfInvoiceParent} WHERE inv_InvoiceAuth_id = '{dataRequestObject.InvOriginalId}' ";
									string urlExecuteQuery = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlExecuteQuery}{sqlUpdate}";

									string a = webClient.DownloadString(urlExecuteQuery);

									jObjectResult.Add($"OK_NEW;{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKyNew}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]};MaTraCuu_{dataResponse["data"]["sobaomat"]}");
								}
							}
						}
					}
				}
				return jObjectResult.ToString();
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", $"{ex.Message}");
				return jObjectResult.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mst"></param>
		/// <param name="userName"></param>
		/// <param name="passWord"></param>
		/// <param name="mauSo"></param>
		/// <param name="kyHieu"></param>
		/// <param name="invoiceNumber"></param>
		/// <param name="xml"></param>
		/// <param name="opt"></param>
		/// <param name="typeOfInvoice">inv_adjustmentType của hóa đơn</param>
		/// <param name="typeUpdate"></param>
		/// <returns></returns>
		public static string CreateInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int typeOfInvoice = 1, int typeUpdate = 1, int useConverFont = 0)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, opt, null, typeOfInvoice, typeUpdate);
			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						JArray jArrayInvoice = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
						if (jArrayInvoice.Count > 0)
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
						}
						else
						{
							JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
							string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlAddApi}";
							string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
							WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
							string rs = webClient.UploadString(url, dataRequest);
							JObject dataResponse = JObject.Parse(rs);
							if (dataResponse.ContainsKey("error"))
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
							}
							else
							{
								if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
								{
									int trangThaiKy = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;

									if (useConverFont == 1)
									{
										jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{dataResponse["data"]["inv_InvoiceAuth_id"]}_{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]}");
									}
									else
									{
										jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]}");
									}


								}
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}

		public static string CreateInvoiceAugges(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int typeOfInvoice = 1, int typeUpdate = 1)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, opt, null, typeOfInvoice, typeUpdate);
			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						JArray jArrayInvoice = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
						if (jArrayInvoice.Count > 0)
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
						}
						else
						{
							JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
							string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlAddApi}";
							string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
							WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
							string rs = webClient.UploadString(url, dataRequest);
							JObject dataResponse = JObject.Parse(rs);
							if (dataResponse.ContainsKey("error"))
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
							}
							else
							{
								if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
								{
									int trangThaiKy = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
									jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}+{dataResponse["data"]["inv_invoiceNumber"]}");
								}
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mst"></param>
		/// <param name="userName"></param>
		/// <param name="passWord"></param>
		/// <param name="mauSo"></param>
		/// <param name="kyHieu"></param>
		/// <param name="invoiceNumber"></param>
		/// <param name="xml"></param>
		/// <param name="opt"></param>
		/// <param name="typeOfInvoice">inv_adjustmentType của hóa đơn</param>
		/// <param name="typeUpdate"></param>
		/// <returns></returns>
		public static string CreateInvoiceVmd(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int typeOfInvoice = 1, int typeUpdate = 1)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, opt, null, typeOfInvoice, typeUpdate);
			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						JArray jArrayInvoice = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
						if (jArrayInvoice.Count > 0)
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
						}
						else
						{
							JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
							string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlAddApi}";
							string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
							WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
							string rs = webClient.UploadString(url, dataRequest);
							JObject dataResponse = JObject.Parse(rs);
							if (dataResponse.ContainsKey("error"))
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
							}
							else
							{
								if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
								{
									int trangThaiKy = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
									jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]};MaTraCuu{dataResponse["data"]["sobaomat"]}");
								}
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="mst"></param>
		/// <param name="userName"></param>
		/// <param name="passWord"></param>
		/// <param name="mauSo"></param>
		/// <param name="kyHieu"></param>
		/// <param name="invoiceNumber"></param>
		/// <param name="xml"></param>
		/// <param name="opt"></param>
		/// <param name="typeOfInvoice"></param>
		/// <param name="typeUpdate"></param>
		/// <returns></returns>
		public static string CreateInvoiceSaveSign(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int typeOfInvoice = 1, int typeUpdate = 1)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, opt, null, typeOfInvoice, typeUpdate);
			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						JArray jArrayInvoice = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
						if (jArrayInvoice.Count > 0)
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
						}
						else
						{
							JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
							string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlAddSignApi}";
							string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
							WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
							string rs = webClient.UploadString(url, dataRequest);
							JObject dataResponse = JObject.Parse(rs);
							if (dataResponse.ContainsKey("error"))
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
							}
							else
							{
								if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
								{
									int trangThaiKy = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
									jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]};MaTraCuu_{dataResponse["data"]["sobaomat"]}");
								}
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}


		public static string CreateInvoiceSse(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, bool opt, int signType, int typeOfInvoice = 1, int typeUpdate = 1)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, opt, signType, typeOfInvoice, typeUpdate);
			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						JArray jArrayInvoice = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
						if (jArrayInvoice.Count > 0)
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
						}
						else
						{
							JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
							string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlAddSseApi}";
							string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
							WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
							string rs = webClient.UploadString(url, dataRequest);
							JObject dataResponse = JObject.Parse(rs);
							if (dataResponse.ContainsKey("error"))
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
							}
							else
							{
								if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
								{
									int trangThaiKy = dataResponse["data"][0]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"][0]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
									jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"][0]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"][0]["inv_invoiceNumber"]}");
								}
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}


		#region Cancel Invoice

		public static string CancelInvoice(string mst, string userName, string passWord, string xmlData)
		{
			JObject jObjectResult = new JObject();

			try
			{
				List<InvoiceCancel> invoiceCancels = DataConvert.GetInvoiceCancels(xmlData);
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlDeleteInvoiceApi}";

				foreach (InvoiceCancel invoiceCancel in invoiceCancels)
				{
					JArray jArrayInvoiceAuth = ApiService.GetInvoice(userName, passWord, invoiceCancel.InvSerial, invoiceCancel.InvPattern, invoiceCancel.InvNumber, mst);

					if (jArrayInvoiceAuth.Count > 0)
					{
						string invoiceAuthId = jArrayInvoiceAuth[0]["inv_InvoiceAuth_id"].ToString();
						JObject jObject = new JObject
						{
							{"inv_InvoiceAuth_id", invoiceAuthId},
							{"sovb", invoiceCancel.SoVb},
							{"ngayvb", invoiceCancel.NgayVb},
							{"ghi_chu", invoiceCancel.GhiChu}
						};
						try
						{
							WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
							string rs = webClient.UploadString(url, jObject.ToString());
							JObject jObjectRs = JObject.Parse(rs);
							if (jObjectRs.ContainsKey("ok"))
							{
								jObjectResult.Add($"OK_{invoiceCancel.InvNumber}", $"Xóa bỏ hóa đơn {invoiceCancel.InvNumber} thành công");
							}
							else
							{
								if (jObjectRs.ContainsKey("error"))
								{
									jObjectResult.Add($"ERROR_{invoiceCancel.InvNumber}", $"{jObjectRs["error"]}");

								}
							}

						}
						catch (Exception ex)
						{
							jObjectResult.Add($"ERROR_{invoiceCancel.InvNumber}", $"{ex.Message}");
						}
					}
					else
					{
						jObjectResult.Add($"ERROR_{invoiceCancel.InvNumber}", $"Không tồn tại số hóa đơn: {invoiceCancel.InvNumber} Mẫu số: {invoiceCancel.InvPattern} Ký hiệu: {invoiceCancel.InvSerial} ! ");
					}
				}
				return jObjectResult.ToString();
			}
			catch (Exception e)
			{
				jObjectResult.Add("ERROR", $"{e.Message}");
				return jObjectResult.ToString();
			}
		}

		#endregion

		public static string GetInvoiceFromDateToDate(string mst, string userName, string passWord, string mauSo,
			string kyHieu, string tuNgay, string denNgay)
		{


			JObject jObjectResult = new JObject();

			if (!CommonService.CheckDate(tuNgay) || !CommonService.CheckDate(denNgay))
			{
				jObjectResult.Add("ERROR", "Ngày không đúng định dạng yyyy-MM-dd");
				return jObjectResult.ToString();
			}
			try
			{
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetInvoiceNumberByDate}";
				JObject data = new JObject
			{
				{"tu_ngay", tuNgay },
				{"den_ngay", denNgay },
				{"mau_so", mauSo },
				{"ky_hieu", kyHieu }
			};

				string result = webClient.UploadString(url, data.ToString());
				JObject response = JObject.Parse(result);


				if (response.ContainsKey("data"))
				{
					JArray dataResponse = JArray.FromObject(response["data"]);
					if (dataResponse.Count > 0)
					{
						JArray jArrayItem = new JArray();
						foreach (JToken token in dataResponse)
						{
							int trangThaiKy = token["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : token["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
							JObject jObjectItem = new JObject
							{
								{"inv_InvoiceAuth_id", token["inv_InvoiceAuth_id"] },
								{"inv_invoiceNumber" , token["inv_invoiceNumber"]},
								{"trang_thai_hd",token["trang_thai_hd"] },
								{"trang_thai" , trangThaiKy},
								{"key", token["so_benh_an"] },
								{"ngayhoadon",  token["inv_invoiceIssuedDate"] }
							};
							jArrayItem.Add(jObjectItem);
						}
						jObjectResult.Add("OK", jArrayItem);
					}
					else
					{
						jObjectResult.Add("ERROR", "Không tìm thấy hóa đơn");
					}
				}
				else if (response.ContainsKey("error"))
				{
					jObjectResult.Add("ERROR", response["error"]);
				}

				return jObjectResult.ToString();
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}

		}

		public static string PreviewPdf(string mst, string userName, string passWord, string mauSo, string kyHieu,
			string invoiceNumber)
		{
			JObject json = new JObject();
			try
			{
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string url =
					$"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetInfo}?invoiceCode={mauSo}&invoiceSeries={kyHieu}&invoiceNumber={invoiceNumber}";
				string response = webClient.DownloadString(url);
				if (!string.IsNullOrEmpty(response))
				{
					JObject jObject = JObject.Parse(response);
					if (jObject.Count > 0)
					{
						if (jObject.ContainsKey("inv_InvoiceAuth_id"))
						{
							string invInvoiceAuthId = jObject["inv_InvoiceAuth_id"].ToString();
							string urlPdf =
								$"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlPreviewPdf}?id={invInvoiceAuthId}";
							json.Add($"OK_{invoiceNumber}", urlPdf);
							return json.ToString();
						}
						else
						{
							if (jObject.ContainsKey("error"))
							{
								json.Add($"ERROR_{invoiceNumber}", jObject["error"]);
								return json.ToString();
							}
						}
					}
					else
					{
						json.Add($"ERROR_{invoiceNumber}", $"Không tìm thấy hóa đơn có mẫu số {mauSo}, ký hiệu {kyHieu}, số hóa đơn {invoiceNumber}");
						return json.ToString();
					}
				}
				else
				{
					json.Add($"ERROR_{invoiceNumber}", $"Không tìm thấy hóa đơn có mẫu số {mauSo}, ký hiệu {kyHieu}, số hóa đơn {invoiceNumber}");
					return json.ToString();
				}
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}

			return null;

		}

		public static string DownloadPdfBase64(string mst, string userName, string passWord, string mauSo, string kyHieu,
			string invoiceNumber)
		{

			string pdfResult = PreviewPdf(mst, userName, passWord, mauSo, kyHieu, invoiceNumber);
			JObject jObject = JObject.Parse(pdfResult);
			if (jObject != null && jObject.ContainsKey($"OK_{invoiceNumber}"))
			{
				if (jObject.ContainsKey($"OK_{invoiceNumber}"))
				{
					HttpClient client = LoginService.SetupHttpClient(userName, passWord, mst);
					byte[] bytes = client.GetAsync(jObject[$"OK_{invoiceNumber}"].ToString()).Result.Content
						.ReadAsByteArrayAsync().Result;
					string result = Convert.ToBase64String(bytes);
					JObject json = new JObject
					{
						{$"OK_{invoiceNumber}", result}
					};

					return json.ToString();
				}
				else
				{
					if (jObject.ContainsKey($"ERROR_{invoiceNumber}"))
					{
						return pdfResult;
					}
				}
			}
			else
			{
				return null;
			}
			return null;
		}


		// Cập nhật ngày 2019-10-23
		public static string GetTbph(string mst, string userName, string passWord)
		{
			JObject json = new JObject();
			try
			{
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetTbph}";

				string rs = webClient.DownloadString(url);
				JArray result = JArray.Parse(rs);
				if (result.Count > 0)
				{
					json.Add("OK", result);
				}
				else
				{
					json.Add("ERROR", "Không tìm thấy thông báo phát hành");
				}

				return json.ToString();
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}
		}

		public static string GetInvoiceById(string mst, string userName, string passWord, string id)
		{
			JObject json = new JObject();
			try
			{
				JObject invoice = ApiService.GetInvoiceById(userName, passWord, mst, id);
				if (invoice.ContainsKey("error"))
				{
					json.Add("ERROR", invoice["error"].ToString());
					return json.ToString();
				}

				json.Add("id", invoice["inv_InvoiceAuth_id"].ToString());
				json.Add("inv_invoiceNumber", invoice["inv_invoiceNumber"]);
				json.Add("key", invoice["so_benh_an"]);
				json.Add("trang_thai", invoice["trang_thai"]);
				json.Add("trang_thai_hd", invoice["trang_thai_hd"]);
				json.Add("mau_so", invoice["mau_hd"]);
				json.Add("ky_hieu", invoice["inv_invoiceSeries"]);
				int trangThaiKy = invoice["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : invoice["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
				json.Add("trang_thai_ky", trangThaiKy);

				JObject result = new JObject
				{
					{$"OK_{id}", json}
				};
				return result.ToString();
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}

		}

		public static string GetListInvoice(string mst, string userName, string passWord, string listId)
		{
			JObject json = new JObject();
			try
			{
				JObject data = ApiService.GetListInvoice(userName, passWord, mst, listId);
				if (data.ContainsKey("error"))
				{
					json.Add("ERROR", data["error"]);
					return json.ToString();
				}
				json.Add("OK", data);
				return json.ToString();
			}
			catch (Exception ex)
			{
				json.Add("ERROR", ex.Message);
				return json.ToString();
			}
		}

		public static string GetInvoiceBravoByKeyApi(string mst, string userName, string passWord, string keyBravo)
		{
			JObject json = new JObject();
			try
			{
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetInvoiceByKeyApiBravo}{keyBravo}";

				string rs = webClient.DownloadString(url);
				JObject result = JObject.Parse(rs);

				if (result.ContainsKey("Inv_InvoiceAuth_id"))
				{
					json.Add("OK", result);
				}
				else
				{
					json.Add("ERROR", "Không tìm thấy hóa đơn");
				}

				return json.ToString();
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}
		}

		public static string GetInvoiceBravoByFKey(string mst, string userName, string passWord, string fKey)
		{
			JObject json = new JObject();
			try
			{
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetInvoiceFkeyApiBravo}";

				JObject data = new JObject
				{
					{"data", fKey}
				};

				string rs = webClient.UploadString(url, data.ToString());
				JObject result = JObject.Parse(rs);
				if (result.ContainsKey("data"))
				{
					json.Add("OK", result);
				}
				else
				{
					json.Add("ERROR", "Không tìm thấy hóa đơn");
				}

				return json.ToString();
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}
		}

		public static string GetInvoiceBravoByDate(string mst, string userName, string passWord, string kyHieu, string mauSo, string tuNgay, string denNgay)
		{
			JObject json = new JObject();
			try
			{
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetInvoiceByDateApiBravo}";

				JObject data = new JObject
				{
					{"tu_ngay", tuNgay},
					{"den_ngay", denNgay},
					{"ky_hieu", kyHieu},
					{"mau_so", mauSo},
				};

				string rs = webClient.UploadString(url, data.ToString());
				JObject response = JObject.Parse(rs);
				if (response.ContainsKey("error"))
				{
					json.Add("ERROR", response["error"]);
				}
				else
				{
					JArray result = (JArray)(response["data"]);
					if (result.Count > 0)
					{
						json.Add("OK", result);
					}
					else
					{
						json.Add("ERROR", "Không tìm thấy hóa đơn");
					}

				}


				return json.ToString();
			}
			catch (Exception e)
			{
				json.Add("ERROR", e.Message);
				return json.ToString();
			}
		}

		public static string CreateInvoiceIPos(string mst, string userName, string passWord, string kyHieu,
			string mauSo, string xml, string token)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, "", xml, false, null);

			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						if (string.IsNullOrEmpty(invoice.Master.Key))
						{
							jObjectResult.Add("ERROR", "Vui lòng nhập key");
							return jObjectResult.ToString();
						}

						JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
						string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UserCreateInvoiceIPos}";
						string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
						WebClient webClient = LoginService.SetupWebClientIPos(mst, token);
						string rs = webClient.UploadString(url, dataRequest);
						JObject dataResponse = JObject.Parse(rs);
						if (dataResponse.ContainsKey("error"))
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
						}
						else
						{
							if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
							{
								int trangThaiKy = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
								jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]}");
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}

		public static string DeleteInvoice(string mst, string userName, string passWord, string id)
		{
			JObject json = new JObject();
			try
			{
				JObject invoiceSearch = ApiService.GetInvoiceById(userName, passWord, mst, id);

				if (invoiceSearch.ContainsKey("error"))
				{
					json.Add($"ERROR_DELETE_{id}", invoiceSearch["error"].ToString());
					return json.ToString();
				}

				string status = invoiceSearch["trang_thai"].ToString();
				if (status.Equals(CommonConstants.DaKy) || status.Equals(CommonConstants.ChoNguoiMuaKy) ||
					status.Equals(CommonConstants.NguoiMuaDaKy))
				{
					json.Add("ERROR_INVOICE_SIGNED",
						$"Hóa đơn có Id = {id} đã ký. Không thể xóa");
					return json.ToString();

				}

				JArray jArray = new JArray();
				JObject a = new JObject
				{
					{"inv_InvoiceAuth_id", id}
				};
				jArray.Add(a);

				JObject dataRequest = new JObject
				{
					{"windowid", "WIN00187"},
					{"editmode", 3},
					{
						"data", jArray
					}
				};
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlSaveApi}";

				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string rs = webClient.UploadString(url, dataRequest.ToString());
				JObject dataResponse = JObject.Parse(rs);

				if (dataResponse.ContainsKey("error"))
				{
					json.Add($"ERROR_DELETE_{id}", $"{dataResponse["error"]}");
					return json.ToString();
				}

				if (dataResponse.ContainsKey("ok"))
				{
					json.Add($"OK_DELETE_{id}",
						$"Xóa hóa đơn có Id: {id} thành công");
				}

				return json.ToString();
			}
			catch (Exception ex)
			{
				json.Add(new JObject
				{
					{"ERROR", ex.Message}
				});
				return json.ToString();
			}
		}


		// Cập nhật 2019-10-25
		public static string SignInvoices(string mst, string userName, string passWord, string listId)
		{
			JObject json = new JObject();
			try
			{
				string[] listIdSign = listId.Split(',');
				JArray array = new JArray();
				foreach (string id in listIdSign)
				{
					JObject item = new JObject
					{
						{"inv_InvoiceAuth_id", id}
					};
					array.Add(item);
				}

				JObject data = new JObject
				{
					{"data", array }
				};

				string dataRequest = data.ToString();
				string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlSignInvoice}";
				WebClient webClient = LoginService.SetupWebClient(userName, passWord, mst);
				string rs = webClient.UploadString(url, dataRequest);
				JObject dataResponse = JObject.Parse(rs);
				if (dataResponse.ContainsKey("error"))
				{
					json.Add("ERROR_SIGN", dataResponse["error"]);
					return json.ToString();
				}
				int count = JArray.Parse(dataResponse["data"].ToString()).Count;
				if (count > 0)
				{
					json.Add("OK_SIGN", dataResponse);
					return json.ToString();
				}
				json.Add("ERROR_SIGN", "Hóa đơn đã ký");
				return json.ToString();
			}
			catch (Exception ex)
			{
				json.Add("ERROR", ex.Message);
				return json.ToString();
			}
		}

		public static string SubstituteInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu, string idOfInvoiceAdjment, string xml, bool opt, int typeOfInvoice = 1, int typeUpdate = 1)
		{
			JObject jObjectResult = new JObject();

			if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(xml) || string.IsNullOrEmpty(idOfInvoiceAdjment))
			{
				jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin");
				return jObjectResult.ToString();
			}

			DataRequestObject dataRequestObject = SetupDataRequestObject(mst, userName, passWord, mauSo, kyHieu, "", xml, opt, null, 3, typeUpdate);
			dataRequestObject.InvOriginalId = idOfInvoiceAdjment;
			try
			{
				List<Invoice> invoices = DataConvert.GetListInvoiceByXml(dataRequestObject.XmlData);

				if (invoices.Count > 0)
				{
					foreach (Invoice invoice in invoices)
					{
						if (string.IsNullOrEmpty(invoice.Master.NumberOrDucument) || string.IsNullOrEmpty(invoice.Master.DateOrDucument) || string.IsNullOrEmpty(invoice.Master.NoteOfDocument))
						{
							jObjectResult.Add($"ERROR", "Vui lòng nhập đủ thông tin điều chỉnh");
							return jObjectResult.ToString();
						}


						JObject invoiceSearch = ApiService.GetInvoiceById(userName, passWord, mst, idOfInvoiceAdjment);
						if (invoiceSearch.ContainsKey("error"))
						{
							jObjectResult.Add($"ERROR", invoiceSearch["error"].ToString());
							return jObjectResult.ToString();
						}

						string trangThai = invoiceSearch["trang_thai"].ToString();
						if (trangThai.Equals(CommonConstants.ChoKy) || trangThai.Equals(CommonConstants.ChoDuyet))
						{
							jObjectResult.Add($"ERROR", "Hóa đơn chưa ký không thể thay thế");
							return jObjectResult.ToString();
						}

						JArray jArrayInvoice = ApiService.GetInvoiceByKey(dataRequestObject.Username, dataRequestObject.Password, dataRequestObject.KyHieu, dataRequestObject.MauSo, invoice.Master.Key, mst);
						if (jArrayInvoice.Count > 0)
						{
							jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key} đã tồn tại");
						}
						else
						{
							JObject jObjectMinvoice = JsonConvert.CreateJsonMinvoice(dataRequestObject, invoice);
							string url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlSubstituteInvoice}";
							string dataRequest = jObjectMinvoice.ToString().Replace("&amp;", "&");
							WebClient webClient = LoginService.SetupWebClient(dataRequestObject.Username, dataRequestObject.Password, mst);
							string rs = webClient.UploadString(url, dataRequest);
							JObject dataResponse = JObject.Parse(rs);
							if (dataResponse.ContainsKey("error"))
							{
								jObjectResult.Add($"ERROR_{invoice.Master.Key}", $"Key {invoice.Master.Key}: {dataResponse["error"]} ");
							}
							else
							{
								if (dataResponse.ContainsKey("ok") && dataResponse.ContainsKey("data"))
								{
									string sqlUpdate =
										$"UPDATE dbo.inv_InvoiceAuth SET trang_thai_hd = 17, inv_adjustmentType = 17 WHERE inv_InvoiceAuth_id = '{idOfInvoiceAdjment}' ";
									string urlExecuteQuery = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlExecuteQuery}{sqlUpdate}";

									string a = webClient.DownloadString(urlExecuteQuery);

									int trangThaiKy = dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.ChoKy) ? 1 : dataResponse["data"]["trang_thai"].ToString().Contains(CommonConstants.DaKy) ? 2 : 3;
									jObjectResult.Add($"OK_{invoice.Master.Key};{dataResponse["data"]["trang_thai_hd"]}_{trangThaiKy}", $"{dataRequestObject.MauSo};{dataRequestObject.KyHieu}-{invoice.Master.Key}_{dataResponse["data"]["inv_invoiceNumber"]}");
								}
							}
						}
					}

					return jObjectResult.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}

		}

		public static string DownloadXmlBase64(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber)
		{
		    JObject jObjectResult = new JObject();
            if (!CheckDataRequest(mst, userName, passWord, mauSo, kyHieu, invoiceNumber))
            {
                jObjectResult.Add("ERROR", "Vui lòng nhập đủ thông tin đầu vào");
                return jObjectResult.ToString();

            }

			try
			{
				JArray jArrayInvoice = ApiService.GetInvoice(userName, passWord, kyHieu, mauSo, invoiceNumber, mst);
				if (jArrayInvoice.Count == 0)
				{
					jObjectResult.Add($"ERROR_{invoiceNumber}", $"Không tìm thấy hóa đơn có mẫu số {mauSo}, ký hiệu {kyHieu}, số hóa đơn {invoiceNumber}");
					return jObjectResult.ToString();
				}

				JToken invoice = (jArrayInvoice.Count == 1
				                     ? jArrayInvoice[0]
				                     : jArrayInvoice.FirstOrDefault(x => x["trang_thai_hd"].ToString().Equals("7"))) ??
				                 jArrayInvoice[0];

			    var soBaoMat = invoice["sobaomat"].ToString();

                var client = new HttpClient();
			    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			    var url = "http://tracuuhoadon.minvoice.vn/tracuu2/getinvoicexml";
			    var data = new JObject
			    {
			        {"sobaomat" , soBaoMat},
			        {"masothue", mst }
			    };

			    var resposne = client.PostAsync(url, new StringContent(data.ToString(), Encoding.UTF8, "application/json")).Result;
			    string result;
			    if (!resposne.IsSuccessStatusCode)
			    {
			         result = resposne.Content.ReadAsStringAsync().Result;
			        jObjectResult.Add($"ERROR_{invoiceNumber}", result);
			        return jObjectResult.ToString();
			    }

			    var bytes = resposne.Content.ReadAsByteArrayAsync().Result;
			     result = Convert.ToBase64String(bytes);

			    jObjectResult.Add($"OK_{invoiceNumber}", result);
			    return jObjectResult.ToString();
			}
			catch (Exception ex)
			{
				jObjectResult.Add("ERROR", ex.Message);
				return jObjectResult.ToString();
			}
		}

	    private static bool CheckDataRequest(string mst, string userName, string passWord, string mauSo, string kyHieu,
	        string invoiceNumber)
	    {
	        if (string.IsNullOrEmpty(mst) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord) || string.IsNullOrEmpty(mauSo) || string.IsNullOrEmpty(kyHieu) || string.IsNullOrEmpty(invoiceNumber))
	        {
	            return false;
	        }

	        return true;
	    }
	}
}