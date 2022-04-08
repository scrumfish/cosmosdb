import React, { useState, useEffect } from 'react'
import { Container, Row, Col } from 'reactstrap'
import { useHistory, useParams } from 'react-router'
import './Blog.css'

const Blog = () => {
  const {id} = useParams()
  const history = useHistory()
  const [blog, setBlog] = useState()

  const getBlog = async () => {
    if (!id) {
      history.push('/')
    }
    const response = await fetch(`/api/blog/${id}`)
    if (response.ok) {
        const json = await response.json()
        setBlog(json)
    } else {
      history.push('/')
    }
  }

  useEffect(() => {
    getBlog()
  },[])

  return (
    <Container fluid>
      {!!blog &&
        <>
          <Row>
            <Col xs={0} sm={0} lg={1} />
            <Col xs={12} sm={12} lg={10}>
              <h1>{blog.title}</h1>
            </Col>
            <Col xs={0} sm={0} lg={1} />
          </Row>
          <Row className='blog-display'>
            <Col xs={0} sm={0} lg={1} />
            <Col xs={12} sm={12} lg={10}>
              <div dangerouslySetInnerHTML={{ __html: blog.article }} />
            </Col>
            <Col xs={0} sm={0} lg={1} />
          </Row>
        </>
      }
    </Container>
  )
}

export default Blog
