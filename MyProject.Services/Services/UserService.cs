using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyProject.Services.Entities;
using MyProject.Services.Interfaces;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Net.Http;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace MyProject.Services.Services
{
    public class UserService : IUserService
    {
        public async Task<Response<List<User>>> FilterUsers(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return Response<List<User>>.GetResult(-1, "formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Response<List<User>>.GetResult(-1, "Not Support file extension");
            }

            var list = new List<User>();

            var stream = new MemoryStream();

            await formFile.CopyToAsync(stream, cancellationToken);

            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                    {
                        //loop all columns in a row
                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {
                            //add the cell data to the List
                            if (worksheet.Cells[i, j].Value != null)
                            {
                                User user = new User();
                                user.FirstName = worksheet.Cells[i, 1].Value.ToString().Trim();
                                user.LastName = worksheet.Cells[i, 2].Value.ToString().Trim();
                                user.Age = int.Parse(worksheet.Cells[i, 4].Value.ToString().Trim());
                                user.Identity = worksheet.Cells[i, 2].Value.ToString().Trim();
                                user.IsValid = IDValidator(user.Identity);
                                if (user.IsValid)
                                    user.Id = UseAPI(user).Id;
                                else
                                    user.Id = -1;
                                list.Add(user);
                            }
                        }
                    }
                }
                return Response<List<User>>.GetResult(0, "OK", list);

            }
        }

        private bool IDValidator(string id)
        {
            int sum = 0, incNum;

            if (id.Length != 9 || int.TryParse(id, out sum))
            {  // Make sure ID is formatted properly
                return false;
            }
            for (int i = 0; i < id.Length; i++)
            {
                incNum = (int)(id[i]) * ((i % 2) + 1);  // Multiply number by 1 or 2
                sum += (incNum > 9) ? incNum - 9 : incNum;  // Sum the digits up and add to total
            }
            return (sum % 10 == 0);
        }

        private async Task<User> UseAPI(User obj)
        {
            HttpClient client = new HttpClient();
            string str = JsonConvert.SerializeObject(obj);

            HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://fakerestapi.azurewebsites.net/api/v1/Users",
                                   content);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<User>();

            return null;
        }
    }
}
