/* global gridMixin */

function LogErrorIndexVue() {
  Vue.component('paginate', VuejsPaginate)

  new Vue({
    el: '#logerror-index-vue',

    mixins: [gridMixin],

    data: {
      url: '/api/logerror',

      // Filtros de búsqueda
      message: '',
      username: '',
      path: '',
      createAt: '',
      checked: '',

      // Datos para el modal
      messageModal: '',
      stackTrace: ''
    },

    mounted() {
      $('#datetimepickerCreateAt').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.createAt = $('#datetimepickerCreateAt').val()
        }.bind(this))
    },

    watch: {
      createAt(val) {
        this.createAt = val
        this._onFilterChange()
      },
      username(val) {
        this.username = val
        this._onFilterChange()
      },
      path(val) {
        this.path = val
        this._onFilterChange()
      },
      message(val) {
        this.message = val
        this._onFilterChange()
      },
      checked(val) {
        this.checked = val
        this._onFilterChange()
      }
    },

    methods: {
      onFilterReset() {
        this.createAt = ''
        this.username = ''
        this.path = ''
        this.message = ''
        this.checked = ''
        this._onFilterChange()
      },

      onModalStackTrace(id) {
        const url = `${this.url}/${id}`

        axios({
          method: 'get',
          url: url,
        })
          .then((response) => {
            const logError = this.findElementById(id)

            this.stackTrace = response.data.stackTrace
            this.messageModal = response.data.message
            if (logError.element.checked === false) {
              logError.element.checked = true

              // Disminuye en 1 en la navegación lateral.
              const el = $('#unread-log-error-count')
              const numUnread = parseInt(el.html())
              if (numUnread > 0) {
                el.html(numUnread - 1)
              }
            }
          })
      },

      onDelete(id) {
        const url = `${this.url}/${id}`
        this._onDelete(url, id)
      },

      _getObjectList() {
        const url = `${this.url}/paginate/`
        this.loading = true

        axios({
          method: 'get',
          url: url,
          params: {
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
            sortOrder: this.sortOrder,
            orderBy: this.orderBy,

            // Filtros de búsqueda
            message: this.message,
            username: this.username,
            path: this.path,
            createAt: this.createAt,
            checked: this.checked
          }
        })
          .then((response) => {
            const data = response.data
            this.objectList = data.objectList
            this.totalItems = data.totalItems
            this.totalPages = data.totalPages
            this.loading = false
          })
      }
    }
  })
}
