using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNotunThikana.Controllers
{
    public class ExecutionController : Controller
    {

        public String Details;

        public String Email;
        class UpdateDetails : Models.Database
        {
            public void Update(String Email,String Details,String Address,String FlatName)
            {
                DatabaseCon("NotunThikana");

                setData("Update GoogleMap set Details='"+Details+"' where Email='"+Email+"' and Address='"+Address+"' and FlatName='"+FlatName     +"'");
            }

            public void Delete(String Email,String Address,String FlatName)
            {
                DatabaseCon("NotunThikana");

                setData("Delete from GoogleMap where Email='" + Email + "' and Address='" + Address + "' and FlatName='" + FlatName + "'");

                DatabaseCon("NotunThikana");

                setData("Delete from ImageDb where Email='" + Email + "' and Address='" + Address + "' and FlatName='" + FlatName + "'");
            }
        }
        // GET: Execution
        public ActionResult Operation()
        {

            if(Session["Email"] == null && Session["Phone"]==null && Session["User"]==null && Session["Name"]==null)
            {
                Response.Redirect("~/HomePage/getHomeData");
            }

            UpdateDetails up = new UpdateDetails();
            String op = Request.Params["exe"].ToString();
            if (op.Equals("update"))
            {
                this.Details = Request.Params["Details"].ToString();

                this.Email = Request.Params["Email"].ToString();
            }
            String Address = Request.Params["Address"].ToString();
            String FlatName = Request.Params["FlatName"].ToString();

            //  String x = Email + " \n " + Address + " \n " + FlatName;

            if (op.Equals("update"))
            {
                up.Update(this.Email, this.Details, Address, FlatName);

                Response.Redirect("~/ProfileLO/ProfileLandOwner#options");
            }
            else if (op.Equals("delete"))
            {
                up.Delete(Session["Email"].ToString(), Address, FlatName);

                Response.Redirect("~/ProfileLO/ProfileLandOwner#FlatTable");
            }
            

            return View();
        }
    }
}