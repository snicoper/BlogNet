/* global simplemde */

function ArticleAdminCreateEditVue() {
  new Vue({
    el: '#article-admin-create-edit-vue',

    data: {
      loadingImage: false,
      urlInsertVideo: ''
    },

    mounted() {
      $('#OwnerId').select2({
        theme: 'bootstrap4',
        minimumInputLength: 1,
        width: '100%',

        ajax: {
          url: '/api/account/search/username',

          data: function (params) {
            return {
              term: params.term
            }
          },

          processResults: function (data) {
            return {
              results: data
            }
          }
        }
      })

      $('#DefaultTagId').select2({
        theme: 'bootstrap4',
        width: '100%'
      })

      $('#Tags').select2({
        theme: 'bootstrap4',
        width: '100%'
      })
    },

    methods: {
      uploadImage(event) {
        this.loadingImage = true
        let data = new FormData()
        data.append('file', event.target.files[0])

        axios.put(
          '/api/article/upload-image',
          data
        ).then((response) => {
          const data = response.data
          if (data.success === true) {
            const imagePath = `/media/blog/articles/posts/${data.returnMessages.filename}`
            let content = `<img class="image-article-uploaded" src="${imagePath}">`

            simplemde.codemirror.focus()
            simplemde.codemirror.replaceSelection(content)
          } else {
            toastr.error('Error al cargar la imagen')
          }
          this.loadingImage = false
        })
      },

      onInsertVideo() {
        if (!this.urlInsertVideo) {
          toastr.error('El campo URL Video no puede estar vacio')
          return
        }

        const content = `<div class="embed-responsive embed-responsive-16by9">\n${this.urlInsertVideo}\n</div>`
        simplemde.codemirror.focus()
        simplemde.codemirror.replaceSelection(content)
        $('#modal-insert-video').modal('hide')
      }
    }
  })
}
