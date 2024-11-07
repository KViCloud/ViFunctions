{{/*
Expand the name of the chart.
*/}}
{{- define "images-builder.fullname" -}}
{{- printf "%s-%s" .Release.Name .Chart.Name | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Common labels for all resources.
*/}}
{{- define "images-builder.labels" -}}
app: {{ include "images-builder.fullname" . }}
{{- end -}}