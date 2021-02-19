using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3_Test
{
    public partial class index : System.Web.UI.Page
    {
        private const string bucketName = "my-first-aws-bucket-2021";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;

        private static IAmazonS3 client;

        protected void Page_Load(object sender, EventArgs e)
        {
            client = new AmazonS3Client("", "", bucketRegion);
            ListAllObjectsInBucket();
        }

        public void WritingAnObject()
        {
            if (!fup.HasFile || string.IsNullOrEmpty(txtInputKey.Text))
            {
                lblText.Text = "Please enter the object Key and select a File...";
                return;
            }
            try
            {
                string NewFileName = DateTime.Now.Ticks + fup.FileName;
                fup.PostedFile.SaveAs(Server.MapPath(@"~\Resources\") + NewFileName);

                string filePath = Server.MapPath(@"~\Resources\" + NewFileName);
                string Extension = filePath.Contains('.') ? filePath.Substring(filePath.LastIndexOf('.') + 1) : "text/plain";

                var putRequest2 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = txtInputKey.Text,
                    FilePath = filePath,
                    ContentType = Extension
                };

                putRequest2.Metadata.Add("x-amz-meta-title", "someTitle");

                PutObjectResponse response2 = client.PutObject(putRequest2);
                if (response2.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    lblText.Text = "Object Saved!";
                    txtInputKey.Text = "";
                    ListAllObjectsInBucket();
                }
            }
            catch (AmazonS3Exception e)
            {
                lblText.Text = string.Concat("Error encountered: ", e.Message);
            }
            catch (Exception e)
            {
                lblText.Text = string.Concat("Error encountered: ", e.Message);

            }
        }

        public void DownloadObject(string Key)
        {
            var getRequest1 = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = Key
            };
            GetObjectResponse response1 = client.GetObject(getRequest1);
            if (response1.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                string Extension = response1.Headers.ContentType;
                string FileName = string.Concat(@"C:\temp\", Key, ".", Extension);
                response1.WriteResponseStreamToFile(FileName, true);
            }
        }

        protected void btnAddObjects_Click(object sender, EventArgs e)
        {
            WritingAnObject();
        }

        public void DeleteObject(string Key)
        {
            var delRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = Key
            };
            DeleteObjectResponse response = client.DeleteObject(delRequest);
            lblText.Text = "Object " + Key + " has been deleted successfully!";
            ListAllObjectsInBucket();
        }

        public void ListAllObjectsInBucket()
        {
            var getRequest = new ListObjectsRequest
            {
                BucketName = bucketName
            };
            ListObjectsResponse response = client.ListObjects(getRequest);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                lblText.Text = "No objects found";

            else
            {
                DataTable dtObjects = new DataTable();
                dtObjects.Columns.Add("NO.");
                dtObjects.Columns.Add("Key");
                dtObjects.Columns.Add("Size (KB)");
                dtObjects.Columns.Add("Last Modified At");

                for (int i = 0; i < response.S3Objects.Count; i++)
                    dtObjects.Rows.Add(i + 1, response.S3Objects[i].Key, response.S3Objects[i].Size / 1024, response.S3Objects[i].LastModified);

                gvObjects.DataSource = dtObjects;
                gvObjects.DataBind();
            }
        }

        protected void btnGetAll_Click(object sender, EventArgs e)
        {
            ListAllObjectsInBucket();
        }

        protected void gvObjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = int.Parse(e.CommandArgument.ToString());
            string Key = gvObjects.Rows[rowIndex].Cells[3].Text;

            if (e.CommandName == "Remove")
                DeleteObject(Key);
            else if (e.CommandName == "Download")
                DownloadObject(Key);
        }

    }


}