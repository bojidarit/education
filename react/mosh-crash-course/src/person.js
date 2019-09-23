// This is the 'Person' class module

export default class Person {
  constructor(name) {
    this.name = name;
  }

  takeAction(action) {
    console.log(`${this.constructor.name} '${this.name}' is ${action}...`);
  }

  walk() {
    this.takeAction("walking");
  }
}
