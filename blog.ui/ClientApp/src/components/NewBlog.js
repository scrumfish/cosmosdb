import React, { useState } from 'react'
import { Container, Row, Col, Button, Form, Input, Label } from 'reactstrap'
import { useHistory } from 'react-router'
import { EditorState, convertToRaw } from 'draft-js'
import { Editor } from 'react-draft-wysiwyg'
import draftToHtml from 'draftjs-to-html'
import '../../node_modules/react-draft-wysiwyg/dist/react-draft-wysiwyg.css'
import Auth from './Auth'

const NewBlog = () => {
  const [title, setTitle] = useState('')
  const [images, setImages] = useState([])
  const [editorState, setEditorState] = useState(EditorState.createEmpty())
  const history = useHistory()

  const handleSubmit = async (e) => {
      e.preventDefault()
      const content = draftToHtml(convertToRaw(editorState.getCurrentContent()))
      const response = await fetch('/api/blog', {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json'
          },
          body: JSON.stringify({ title: title, article: content, images: images })
      })
      if (response.ok) {
        const json = await response.json()
        history.push(`/blog/${json.id}`)
      }
  }

  const onUpload = async (e) => {
    const formData = new FormData()
    formData.append('formFile', e)
    formData.append('fileName', e.name)
    setImages(images => [...images, e.name])
    const response = await fetch('/api/file', {
      method: 'POST',
      body: formData
    })
    if (response.ok) {
      return response.json()
    }
  }

  return (
    <Container fluid>
      <Row>
        <Col>
          <h4>New Item</h4>
        </Col>
      </Row>
      <Row>
        <Col>
          <Form onSubmit={handleSubmit}>
            <Label for='title'>Title</Label>
            <Input name='title' id='title' value={title} required onChange={e => setTitle(e.target.value)} />
            <Label for='content'>Content</Label>
            <div className='editor-container'>
              <Editor
                editorState={editorState}
                onEditorStateChange={(e) => setEditorState(e)}
                toolbar={
                  {image: {
                    urlEnabled: false,
                    uploadEnabled: true,
                    uploadCallback: onUpload
                  }}
                }
              />
            </div>
            <Button type='submit' color='primary' title='Post'>Post</Button>
          </Form>
        </Col>
      </Row>
    </Container>
  )
}

export default Auth(NewBlog, {roles: ['admin']})
