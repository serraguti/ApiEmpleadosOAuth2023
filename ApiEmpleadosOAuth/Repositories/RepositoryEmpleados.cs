using ApiEmpleadosOAuth.Data;
using ApiEmpleadosOAuth.Models;

namespace ApiEmpleadosOAuth.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return this.context.Empleados.ToList();
        }

        public Empleado FindEmpleado(int id)
        {
            return
                this.context.Empleados.FirstOrDefault(z => z.IdEmpleado == id);
        }

        public List<Empleado> GetEmpleadosDepartamento(int iddepartamento)
        {
            return
                this.context.Empleados.Where(z => z.Departamento == iddepartamento)
                .ToList();
        }

        //NECESITAMOS UN METODO PARA COMPROBAR SI EXISTE UN EMPLEADO
        //POR SU USUARIO (APELLIDO) Y PASSWORD (EMP_NO)
        public Empleado ExisteEmpleado(string apellido, int idempleado)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.Apellido == apellido
                           && datos.IdEmpleado == idempleado
                           select datos;
            return consulta.FirstOrDefault();
        }
    }
}
