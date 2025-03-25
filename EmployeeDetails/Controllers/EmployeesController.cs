using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeDetails.Controllers
{
    
    public class EmployeesController : ApiController
    {
        [BasicAuthentication] 
        public HttpResponseMessage Get()
        {
             string username =  Thread.CurrentPrincipal.Identity.Name;
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                switch(username.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e=> e.Gender.ToLower()== "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower()== "female") .ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
        }
        
        public HttpResponseMessage LoadAllEmployeeByID(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID  : =  " + id.ToString() + " Not found");
                }

            }

        }
        public HttpResponseMessage post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);

                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)

                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id : =" + id.ToString() + "NOt found ");



                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        return Request.CreateResponse(HttpStatusCode.OK, entities.SaveChanges());
                    }


                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }


        }
        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID : = " + id.ToString() + "Not found");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;



                        return Request.CreateResponse(HttpStatusCode.OK, entities.SaveChanges());
                    }


                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
