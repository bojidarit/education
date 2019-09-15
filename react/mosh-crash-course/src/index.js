// import React from "react";
// import ReactDOM from "react-dom";
// const element = <h1>Hello World...</h1>;
// ReactDOM.render(element, document.getElementById("root"));

// JavaScript for React Developers | Mosh
// https://youtu.be/NCwa_xi0Uuc

//-----------------------------------------------------------------------------
// var, let and const
//-----------------------------------------------------------------------------

// var -> function scope
// let -> block scope
function loopDemo() {
  // for (var i = 0; i < 5; i++) {
  for (let i = 0; i < 5; i++) {
    console.log(`From loop -> ${i}`);
  }

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
  const sqareFunc = number => number * number;
  let number = 5;
  console.log(`Sqare of ${number} equals ${sqareFunc(number)}`);

  const persons = [
    { id: 1, name: "first", isActive: true },
    { id: 2, name: "second", isActive: false },
    { id: 3, name: "third", isActive: true }
  ];

  console.log(persons);
  console.log(persons.filter(item => item.isActive));
}

//-----------------------------------------------------------------------------
// arrow functions and 'this'
//-----------------------------------------------------------------------------

const person = {
  talk() {
    console.log("talk this", this);
  },
  walk() {
    var self = this;
    setTimeout(function() {
      console.log("walk self", self);
    }, 100);
  },
  bless() {
    setTimeout(() => console.log("bless arrow this", this), 1000);
  }
};

person.talk();
person.walk();
person.bless();

//-----------------------------------------------------------------------------
// array.map()
//-----------------------------------------------------------------------------

const colors = ["red", "green", "blue"];
const colorItems = colors.map(color => `<li>${color}</li>`);
console.log(colorItems);
