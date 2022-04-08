import React, { useState, useEffect } from 'react'
import { Container } from 'reactstrap'
import { NavMenu } from './NavMenu'
import { getSession } from '../utils/Sessions'

const Layout = ({children}) => {
  const [loggedIn, setLoggedIn] = useState(getSession() !== null)

  useEffect(() => {
    window.onstorage = () => {
      const session = getSession()
      setLoggedIn(session !== null)
    }
  },[])

  return (
    <div>
      <NavMenu loggedIn={loggedIn} />
      <Container>
        {children}
      </Container>
    </div>
  )
}

export default Layout
