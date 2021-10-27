def message = "";
pipeline {
    agent any
    environment {
        BuildConfiguration='Debug'
    }
    stages {
        stage('Build') {
            steps {
                sh 'dotnet build -c $BuildConfiguration'
            }
        }
        
        stage('Push packages') {
            steps {
                sh 'dotnet pack -c $BuildConfiguration "Tgs.Interceptors/Tgs.Interceptors.csproj" --no-build'
                sh 'dotnet nuget push "Tgs.Interceptors/bin/$BuildConfiguration/*.nupkg" -s "raypro-baget" -k admin --skip-duplicate'
            }
        }
    }

    post {
        always {
            step([$class: 'MSTestPublisher', testResultsFile:"**/*.trx", failOnError: false, keepLongStdio: true])
        }
        success {
             sendTelegram("Pipeline: ${currentBuild.fullDisplayName} OK! ${message} \n${currentBuild.getBuildCauses()[0].shortDescription}\n")
        }
        failure {
            sendTelegram("<b>Failed Pipeline:</b> ${currentBuild.fullDisplayName} Something is wrong with ${env.BUILD_URL}/console\n${currentBuild.getBuildCauses()[0].shortDescription}\n")
        }
    }
}

def sendTelegram(message) {
    def encodedMessage = URLEncoder.encode(message, "UTF-8")

    withCredentials([string(credentialsId: "telegramToken", variable: "TOKEN"), string(credentialsId: "telegramChatId", variable: "CHAT_ID")]) {

        response = httpRequest (consoleLogResponseBody: true,
                contentType: 'APPLICATION_JSON',
                httpMode: 'GET',
                url: "https://api.telegram.org/bot${TOKEN}/sendMessage?text=${encodedMessage}&chat_id=${CHAT_ID}",
                validResponseCodes: '200')
        return response
    }
}