{{- define "app.fullname" -}}
{{- printf "%s-%s" .Release.Name .Chart.Name | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{- define "app.name" -}}
{{- default .Chart.Name .Values.nameOverride -}}
{{- end -}}

{{- define "app.chartName" -}}
{{- .Chart.Name -}}
{{- end -}}

{{- define "app.version" -}}
{{- .Chart.Version -}}
{{- end -}}

{{- define "app.appVersion" -}}
{{- .Chart.AppVersion -}}
{{- end -}}

{{- define "app.labels" -}}
helm.sh/chart: {{ include "app.chartName" . }}-{{ include "app.version" . }}
app.kubernetes.io/name: {{ include "app.chartName" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end -}}