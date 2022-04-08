import React from 'react';
import { Redirect } from 'react-router-dom'
import { getSession } from '../utils/Sessions'

function Auth(WrappedComponent, options) {
  return class ModelContainer extends React.Component {
    render() {
      const token = getSession()
      let allowed = false
      if (options && options.roles) {
        options.roles.forEach(r => {
          allowed = token && (allowed || token.roles.indexOf(r) > -1)
        })
      } else {
        allowed = true
      }
      if (!token || !allowed) {
        return ( <Redirect to={'/'} /> )
      } else {
        return (
          <WrappedComponent />
        )
      }
    }
  }
}

export default Auth