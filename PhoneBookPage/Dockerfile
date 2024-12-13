# Use a imagem oficial do Node para build
FROM node:18 as build

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos do projeto
COPY package*.json ./
RUN npm install
COPY . .

# Gera a build do Vue
RUN npm run build

# Usa uma imagem NGINX para servir a build
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 5173

# Comando para rodar o servidor NGINX
CMD ["nginx", "-g", "daemon off;"]
