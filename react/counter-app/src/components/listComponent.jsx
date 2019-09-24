import React, { Component } from "react";

/***
 * Rendering List (conditionally) example
 */
class List extends Component {
  state = {
    tags: []
  };

  renderTags() {
    if (this.state.tags.length === 0) {
      return <p>There are no tags.</p>;
    }

    return (
      <ul>
        {this.state.tags.map(tag => (
          <li key={tag}>{tag}</li>
        ))}
      </ul>
    );
  }

  render() {
    return (
      <React.Fragment>
        {this.state.tags.length === 0 && "Please add some tags."}
        {this.renderTags()}
      </React.Fragment>
    );
  }
}

export default List;
