import React from 'react'
import {Row, Col, Button} from 'reactstrap'
import {Link, useHistory} from 'react-router-dom'
import './BlogItem.css'

const BlogItem = ({blog}) => {
  const history = useHistory()

  const onClick = () => {
    history.push(`/blog/${blog.id}`)
  }

  return (
    <Row className='landing-item'>
      <Col sm={12} xs={12} lg={10}>
        <Link to={`/blog/${blog.id}`}>{blog.title}</Link>
        <p>{blog.fragment}...</p>
      </Col>
      <Col sm={12} xs={12} lg={2} className='landing-item-button'>
        <Button onClick={onClick} color='primary'>See More...</Button>
      </Col>
    </Row>
  )
}

export default BlogItem
