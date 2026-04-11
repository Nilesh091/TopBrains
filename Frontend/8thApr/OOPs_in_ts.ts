class Employee{
  id:number;
  name:string;
  salary:number;
  constructor(id:number,name:string,salary:number){
    this.id=id;
    this.name=name;
    this.salary=salary;
  }

  display():void{
    console.log("Id: ",this.id);
    console.log("Name: ",this.name);
    console.log("Salary: ",this.salary);

  }
}

let emp1 = new Employee(1,"John Doe",50000);
emp1.display(); 