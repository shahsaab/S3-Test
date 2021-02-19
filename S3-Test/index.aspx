<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="S3_Test.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>Create Object</h3>
            <asp:FileUpload ID="fup" runat="server" Enabled="true" />
            <asp:TextBox ID="txtInputKey" placeholder="Enter object Key" runat="server"></asp:TextBox>
            <asp:Button ID="btnAddObjects" Text="Add Objects" runat="server" OnClick="btnAddObjects_Click" />
            <br />
            <br />

            <h3>View Objects</h3>
            <asp:Button ID="btnGetAll" Text="Get All Objects" runat="server" OnClick="btnGetAll_Click" />
            <asp:GridView ID="gvObjects" runat="server" Width="50%" OnRowCommand="gvObjects_RowCommand">
                <Columns>
                    <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Remove" Text="Delete" ControlStyle-BackColor="Red" ControlStyle-ForeColor="White" />
                    <asp:ButtonField ButtonType="Link" HeaderText="Download" CommandName="Download" Text="Download" />
                </Columns>
                <EmptyDataTemplate>
                    No objects found...
                </EmptyDataTemplate>
            </asp:GridView>


            <div style="position: absolute; padding-bottom:20px; bottom: 0; left: 0; width:100%;background-color:azure; font-size:xx-large">
                Status:
                <asp:Label ID="lblText" runat="server"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
