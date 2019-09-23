import Person from "./person";

// This is a named export
// One must put its name in curly bracelets like this:
// import { ... } from "...";
export function demoFunction() {
  console.log("This is a named export demo.");
}

// ----------------------------------------
// This is the 'Teacher' class module
// ----------------------------------------

// This is a Default export
// One can import it with no curly bracelets
// Example: import ...  from "...";
export default class Teacher extends Person {
  constructor(name, degree) {
    super(name);
    this.degree = degree;
  }

  teach() {
    super.takeAction("teaching");
  }
}
