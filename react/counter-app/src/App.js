import React, { Component } from "react";
import NavBar from "./components/navbar";
import Counters from "./components/counters";

class App extends Component {
  state = { counters: [{ id: -1, value: -1 }] };

  logMethodCall(methodName) {
    console.log(`${this.constructor.name} -> ${methodName}`, this.props);
  }

  getDummyState() {
    return [
      { id: 1, value: 3 },
      { id: 2, value: 0 },
      { id: 3, value: 0 },
      { id: 4, value: 0 }
    ];
  }

  // Hook - The constructor is called once in the app life cycle
  // It is the perfect place to initialize the component state
  // 'props' must be passed as constructor parameter in case one wants to use it here
  constructor(props) {
    super(props);
    this.logMethodCall("constructor");
    // The component state can be set here
    const counters = this.getDummyState();
    this.state = { counters };
  }

  // Hook - Called after the component is rendered into the DOM
  // Perfect place to make AJAX calls to get data from the server
  componentDidMount() {
    this.logMethodCall(this.componentDidMount.name);

    // const counters = this.getDummyState();
    // this.setState({ counters });
  }

  // Delete event handler
  handleDelete = counterId => {
    const counters = this.state.counters.filter(c => c.id !== counterId);
    this.setState({ counters });
  };

  // Reset event handler
  handleReset = () => {
    const counters = this.state.counters.map(c => {
      c.value = 0;
      return c;
    });
    this.setState({ counters });
  };

  // Increment event handler
  handleIncrement = counter => {
    // Mosh's cleaned way
    const counters = [...this.state.counters];
    const index = counters.indexOf(counter);
    counters[index] = { ...counter };
    counters[index].value++;
    this.setState({ counters });
  };

  // Decrement event handler
  handleDecrement = counter => {
    // Mine way that loops through all counter until it find the one which value should be modified
    const counters = [...this.state.counters];
    counters.map(c => {
      if (c.id === counter.id && c.value > 0) {
        c.value--;
      }
      return c;
    });
    this.setState({ counters });
  };

  render() {
    this.logMethodCall(this.render.name);

    return (
      <React.Fragment>
        <NavBar
          totalCounters={this.state.counters.filter(c => c.value > 0).length}
        />
        <main className="container">
          <Counters
            counters={this.state.counters}
            onReset={this.handleReset}
            onDelete={this.handleDelete}
            onIncrement={this.handleIncrement}
            onDecrement={this.handleDecrement}
          />
        </main>
      </React.Fragment>
    );
  }
}

export default App;
