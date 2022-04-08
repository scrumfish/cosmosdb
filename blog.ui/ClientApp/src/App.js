import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home'
import Login from './components/Login'
import NewBlog from './components/NewBlog';
import Blog from './components/Blog'

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route exact path='/login' component={Login} />
            <Route exact path='/new' component={NewBlog} />
            <Route path='/blog/:id' component={Blog} />
      </Layout>
    );
  }
}
