import React, { useEffect, useState } from 'react';
import { Container } from 'reactstrap'
import BlogItem from './BlogItem'

const Home = () => {
  const [blogs, setBlogs] = useState([])

  const getBlogTitles = () => {
    fetch('/api/blog')
    .then(response => {
      if (response.ok) {
        response.json()
        .then(json => {
          setBlogs(json)
        })
      }
    })
  }

  useEffect(() => {
    getBlogTitles()
  },[])

  return (
    <Container fluid>
      {!!blogs &&
        blogs.map((b,i) => <BlogItem key={i} blog={b} />)
      }
    </Container>
  )
}

export default Home
