/* globals gridMixin */

function ContactAdminListVue() {
  Vue.component('paginate', VuejsPaginate)

  new Vue({
    el: '#contact-admin-list-vue',

    mixins: [gridMixin],

    data: {
      url: '/api/contact',

      // Filtros de búsqueda
      emailFrom: '',
      subject: '',
      message: '',
      sendAt: '',
      hasRead: 'None'
    },

    mounted() {
      $('#datetimepickerSendAt').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.sendAt = $('#datetimepickerSendAt').val()
        }.bind(this))
    },

    watch: {
      emailFrom(val) {
        this.emailFrom = val
        this._onFilterChange()
      },
      subject(val) {
        this.subject = val
        this._onFilterChange()
      },
      message(val) {
        this.phoneNumber = val
        this._onFilterChange()
      },
      sendAt(val) {
        this.sendAt = val
        this._onFilterChange()
      },
      hasRead(val) {
        this.hasRead = val
        this._onFilterChange()
      }
    },

    methods: {
      onFilterReset() {
        this.emailFrom = ''
        this.subject = ''
        this.message = ''
        this.sendAt = ''
        this.hasRead = 'None'
        this._onFilterChange()
      },

      onDelete(id) {
        const url = `${this.url}/${id}`
        this._onDelete(url, id)
      },

      getLinkContactDetails(id) {
        return `/admin/contact/details/${id}/`
      },

      _getObjectList() {
        const url = `${this.url}/paginate/`
        this.loading = true

        axios({
          method: 'get',
          url: url,
          params: {
            // Paginación
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
            sortOrder: this.sortOrder,
            orderBy: this.orderBy,

            // Filtros de búsqueda
            emailFrom: this.emailFrom,
            subject: this.subject,
            message: this.message,
            sendAt: this.sendAt,
            hasRead: this.hasRead
          },
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
