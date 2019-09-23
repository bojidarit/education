// import React from "react";
// import ReactDOM from "react-dom";
// const element = <h1>Hello World...</h1>;
// ReactDOM.render(element, document.getElementById("root"));

// JavaScript for React Developers from Mosh
// https://youtu.be/NCwa_xi0Uuc

//-----------------------------------------------------------------------------
// var, let and const
//-----------------------------------------------------------------------------

// var -> function scope
// let -> block scope
function loopDemo() {
  let result = "";
  // for (var i = 0; i < 5; i++) {
  for (let i = 0; i < 5; i++) {
    result += `${i} `;
  }

  console.log(result);

  //console.log(`Outside loop -> ${i}`);
}
loopDemo();

// const -> constant (block scope)
const PI = 3.14;
console.log(PI);
//PI = 3.1;

//-----------------------------------------------------------------------------
// arrow functions
//-----------------------------------------------------------------------------

const pi = () => PI;
console.log(`PI func returns ${pi()}`);

{
  const squareFunc = number => number * number;
  let number = 5;
  console.log(`Square of ${number} equals ${squareFunc(number)}`);

  const persons = [
    { id: 1, name: "first", isActive: true },
    { id: 2, name: "second", isActive: false },
    { id: 3, name: "third", isActive: true }
  ];

  console.log("All Jobs:");
  console.log(persons);
  console.log("Active Jobs:");
  // The old way
  console.log(
    persons.filter(function(item) {
      return item.isActive;
    })
  );
  console.log(persons.filter(item => item.isActive));
}

//-----------------------------------------------------------------------------
// arrow functions and 'this'
//-----------------------------------------------------------------------------

const person = {
  talk() {
    console.log("Person talk this", this);
  },
  walk() {
    var self = this;
    setTimeout(function() {
      console.log("Person walk self", self);
    }, 1);
  },
  bless() {
    setTimeout(() => console.log("Person bless arrow this", this), 1);
  }
};

person.talk();
// person.walk();
// person.bless();

//-----------------------------------------------------------------------------
// array.map() using arrow function and template literal
//-----------------------------------------------------------------------------

const colors = ["red", "green", "blue"];
const colorItems = colors.map(color => `<li>${color}</li>`);
console.log("Colors mapped to list item tags:");
console.log(colorItems);

//-----------------------------------------------------------------------------
// Object destructuring
//-----------------------------------------------------------------------------

const address = {
  street: "Main",
  city: "Sofia",
  country: "Bulgaria"
};

console.log("Address object: ");
console.log(address);

const { street: st, city: ct } = address;
console.log(`Address deconstructed : ${ct}, ${st} Str.`);

//-----------------------------------------------------------------------------
// Spread operator
//-----------------------------------------------------------------------------
const arr1 = [1, 2, 3];
const arr2 = [3, 4, 5];

//const clone = [...arr1];

const comboClassicWay = arr1.concat(arr2);
const comboSpreadWay = [...arr1, "a", ...arr2, "b"];

console.log(comboClassicWay);
console.log("Spread arrays: ");
console.log(comboSpreadWay);

const obj1 = { name: "Ivan" };
const obj2 = { job: "Coder" };

const obj1Clone = { ...obj1 };
obj1Clone.lastName = "van Dam";

console.log("Clone objects: ");
console.log(obj1);
console.log(obj1Clone);

console.log("Spread objects: ");
const spreadObj = { ...obj1, sex: "male", ...obj2 };
console.log(spreadObj);

//-----------------------------------------------------------------------------
// Classes
//-----------------------------------------------------------------------------{
class Person {
  constructor(name) {
    this.name = name;
  }

  walk() {
    console.log(`${this.name} is walking...`);
  }
}

const personIvan = new Person("Ivan");

console.log("Person Class: ");
console.log(personIvan);
personIvan.walk();

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------
