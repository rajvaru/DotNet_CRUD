﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Addressbook
{
    public partial class StateAddEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindCountryDropdown();

                if (Request.QueryString["StateID"] != null)
                {
                    EditState(Convert.ToInt32(Request.QueryString["StateID"]));
                }
            }

        }
        public void EditState(int StateID)
        {
            SqlConnection objConn = new SqlConnection("Data Source=LAPTOP-NUIFP4D9\\SQLEXPRESS;Initial Catalog=AddressBook;Integrated Security=true;");

            //SqlConnection objConn = new SqlConnection("Data Source=LAPTOP-NUIFP4D9\\SQLEXPRESS;Initial Catalog=AddressBook;Integrated Security=True;");
            objConn.Open();
            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.Text;
            objCmd.CommandText = "SELECT StateID, StateName, StateCode FROM [dbo].[State] WHERE StateID = " + StateID;         
            SqlDataReader dr = objCmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            objConn.Close();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    txtStateID.Text = drow["StateID"].ToString();
                    txtStateName.Text = drow["StateName"].ToString();
                    txtStateCode.Text = drow["StateCode"].ToString();
                    break;
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection objConn = new SqlConnection("Data Source=LAPTOP-NUIFP4D9\\SQLEXPRESS;Initial Catalog=AddressBook;Integrated Security=true;");
            objConn.Open();
            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.Text;
            if (Request.QueryString["StateID"] != null)
            {
                objCmd.CommandText = "UPDATE [dbo].[State] SET StateName='" + txtStateName.Text.Trim() + "', StateCode='" + txtStateCode.Text.Trim() + "' WHERE StateID=" + Request.QueryString["StateID"].ToString();
                objCmd.ExecuteNonQuery();
            }
            else
            {
                objCmd.CommandText = "INSERT INTO [dbo].[State] (StateName, StateCode) VALUES ('" + txtStateCode.Text.Trim() + "','" +
                    txtStateName.Text.Trim() + "','" + txtStateCode.Text.Trim() + "')";
                objCmd.ExecuteNonQuery();
            }
            objConn.Close();
            Response.Redirect("~/State.aspx");

        }
        private void BindCountryDropdown()
        {
            SqlConnection objConn = new SqlConnection("Data Source=LAPTOP-NUIFP4D9\\SQLEXPRESS;Initial Catalog=AddressBook;Integrated Security=true;");
            objConn.Open();

            SqlCommand objCmd = new SqlCommand("SELECT CountryID, CountryName FROM dbo.Country", objConn);
            SqlDataReader dr = objCmd.ExecuteReader();

            ddlCountryID.DataSource = dr;
            ddlCountryID.DataValueField = "CountryID";
            ddlCountryID.DataTextField = "CountryName";
            ddlCountryID.DataBind();

            ddlCountryID.Items.Insert(0, new ListItem("Select Country", "0"));

            objConn.Close();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            SqlConnection objConn = new SqlConnection("Data Source=LAPTOP-NUIFP4D9\\SQLEXPRESS;Initial Catalog=AddressBook;Integrated Security=true;");
            objConn.Open();

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.Text;

            int countryId = Convert.ToInt32(ddlCountryID.SelectedValue); // Get the selected CountryID
            objCmd.CommandText = "INSERT INTO [dbo].[State] (StateName, StateCode, CountryID) VALUES (@StateName, @StateCode, @CountryID)";

            objCmd.Parameters.AddWithValue("@StateName", txtStateName.Text.Trim());
            objCmd.Parameters.AddWithValue("@StateCode", txtStateCode.Text.Trim());
            objCmd.Parameters.AddWithValue("@CountryID", countryId); 

            objCmd.ExecuteNonQuery();
            objConn.Close();

            Response.Redirect("~/State.aspx");

        }
    }
}