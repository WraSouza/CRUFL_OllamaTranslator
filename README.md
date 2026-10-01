# CRUFL_OllamaTranslator

Uma UFL (User Function Library) desenvolvida em C# (.NET Framework 4.8) para estender as capacidades do Crystal Reports e do SAP Business One, permitindo a tradução dinâmica de rótulos e textos de relatórios através de chamadas a uma Web API middleware (.NET / Docker) conectada a um modelo de IA local (Ollama / Qwen2.5).

🎯 Arquitetura da Solução
O motor do Crystal Reports não possui suporte nativo para chamadas HTTP REST ou consumo de APIs. Esta UFL atua como uma ponte de comunicação COM (Component Object Model), abstraindo a integração web e expondo funções customizadas diretamente no editor de fórmulas do Crystal Reports.

```text
[ Crystal Reports / SAP B1 Client ]
                 │
                 ▼ (Chamada COM)
  [ CRUFL_OllamaTranslator.dll ]
                 │
                 ▼ (HTTP POST REST)
[ Middleware Web API .NET / Docker ]
                 │
                 ▼ (Inference)
    [ Ollama Engine Qwen2.5:3b ]
```



🚀 FuncionalidadesTradução On-the-Fly: Traduz textos e rótulos de relatórios em tempo de execução com base no país/idioma do parceiro de negócios.Compatibilidade Híbrida (32-bit e 64-bit): Compilado em Any CPU para suportar nativamente tanto o Crystal Reports Designer (32-bit) quanto o SAP Business One Client (64-bit).   Deployment Automatizado via GPO: Estrutura preparada para distribuição em massa nos computadores da rede sem necessidade de intervenção do utilizador.Mecanismo de Cache de Memória: Otimização de requisições à API para evitar chamadas redundantes durante a renderização de relatórios extensos.🛠️ Requisitos e Pré-requisitosPlataforma do Projeto: .NET Framework 4.8.Ambiente de Execução:Crystal Reports 2016 / 2020 ou superior.   SAP Business One Client (x86 ou x64).   Dependências de Servidor:Container Docker executando a Web API Middleware e o serviço Ollama.💻 Compilação e BuildPara garantir que o assembly seja carregado sem erros nos ambientes de 32 e 64 bits:Abra a solução no Visual Studio.Aceda às propriedades do projeto CRUFL_OllamaTranslator -> Build.Defina o Platform target como Any CPU.Certifique-se de que a opção Make assembly COM-Visible está ativa ([ComVisible(true)]).Execute a compilação em modo Release.⚙️ Instalação e Registo COMPara que o Crystal Reports e o SAP Business One reconheçam as funções, a DLL deve ser copiada para os diretórios do sistema e registrada através do regasm.exe em ambos os Frameworks.   

📊 Utilização no Crystal Reports
Após o registo bem-sucedido, as funções expostas pela DLL estarão disponíveis no Editor de Fórmulas do Crystal Reports em Funções Adicionais (Additional Functions) -> User Defined Functions (UFL).

Exemplo de Fórmula no Crystal Syntax:
```
// Fórmula: lbl_DataPedido
If IsNull({Comando.PaisPN}) Or {Comando.PaisPN} = "BR" Then
    "Data do Pedido"
Else
    // Chama a UFL passando o texto original e o idioma de destino
    FixuflOllamaTranslate("Data do Pedido", {Comando.PaisPN})
```
