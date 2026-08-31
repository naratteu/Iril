// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 2 "Iril/IR/IR.jay"
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Linq;

using Iril.Types;

#pragma warning disable 219,414

namespace Iril.IR
{
	public partial class Parser
	{
#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = new StringWriter ();

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /* An EOF token */
  public int eof_token;
  
  public int yacc_verbose_flag;

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
//t  internal yydebug.yyDebug debug;

  protected const int yyFinal = 9;
//t // Put this array into a separate class so it is only initialized if debugging is actually used
//t // Use MarshalByRefObject to disable inlining
//t class YYRules : MarshalByRefObject {
//t  public static readonly string [] yyRule = {
//t    "$accept : module",
//t    "module : module_parts",
//t    "module_parts : module_part",
//t    "module_parts : module_parts module_part",
//t    "module_part : SOURCE_FILENAME '=' STRING",
//t    "module_part : TARGET DATALAYOUT '=' STRING",
//t    "module_part : TARGET TRIPLE '=' STRING",
//t    "module_part : LOCAL_SYMBOL '=' TYPE literal_structure",
//t    "module_part : LOCAL_SYMBOL '=' TYPE OPAQUE",
//t    "module_part : function_definition",
//t    "module_part : function_declaration",
//t    "module_part : global_variable",
//t    "module_part : ATTRIBUTES ATTRIBUTE_GROUP_REF '=' '{' attributes '}'",
//t    "module_part : ATTRIBUTES ATTRIBUTE_GROUP_REF '=' '{' '}'",
//t    "module_part : META_SYMBOL_DEF '=' '!' '{' '}'",
//t    "module_part : META_SYMBOL_DEF '=' '!' '{' metadata '}'",
//t    "module_part : META_SYMBOL_DEF '=' META_SYMBOL '(' metadata_args ')'",
//t    "module_part : META_SYMBOL_DEF '=' DISTINCT '!' '{' metadata '}'",
//t    "module_part : META_SYMBOL_DEF '=' DISTINCT META_SYMBOL '(' metadata_args ')'",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage visibility_style global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' SECTION STRING",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' SECTION STRING",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' SECTION STRING ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility_style function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility_style global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type",
//t    "global_kind : GLOBAL",
//t    "global_kind : CONSTANT",
//t    "linkage : EXTERNAL",
//t    "linkage : AVAILABLE_EXTERNALLY",
//t    "linkage : INTERNAL",
//t    "linkage : LINKONCE",
//t    "linkage : LINKONCE_ODR",
//t    "linkage : WEAK",
//t    "linkage : WEAK_ODR",
//t    "linkage : APPENDING",
//t    "linkage : COMMON",
//t    "visibility : PRIVATE",
//t    "visibility_style : HIDDEN",
//t    "metadata_args : metadata_arg",
//t    "metadata_args : metadata_args ',' metadata_arg",
//t    "metadata_arg : SYMBOL ':' metadata_arg_expr",
//t    "metadata_arg : TYPE ':' metadata_arg_expr",
//t    "metadata_arg : ALIGN ':' constant",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL '(' metadata_value_args ')'",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL '(' ')'",
//t    "metadata_arg_expr : metadata_arg_or_expr",
//t    "metadata_arg_or_expr : metadata_arg_and_expr",
//t    "metadata_arg_or_expr : metadata_arg_or_expr '|' metadata_arg_and_expr",
//t    "metadata_arg_and_expr : metadata_arg_primary",
//t    "metadata_arg_primary : SYMBOL",
//t    "metadata_arg_primary : META_SYMBOL",
//t    "metadata_arg_primary : STRING",
//t    "metadata_arg_primary : constant",
//t    "metadata_arg_primary : typed_constant",
//t    "metadata_arg_primary : NULL",
//t    "metadata_kvs : META_SYMBOL META_SYMBOL",
//t    "metadata_kvs : metadata_kvs META_SYMBOL META_SYMBOL",
//t    "metadata : metadatum",
//t    "metadata : metadata META_SYMBOL",
//t    "metadata : metadata ',' typed_value",
//t    "metadata : metadata ',' META_SYMBOL",
//t    "metadata : metadata ',' NULL",
//t    "metadatum : typed_value",
//t    "metadatum : META_SYMBOL",
//t    "metadatum : NULL",
//t    "attributes : attribute",
//t    "attributes : attributes attribute",
//t    "attribute : NORECURSE",
//t    "attribute : NOUNWIND",
//t    "attribute : READNONE",
//t    "attribute : SPECULATABLE",
//t    "attribute : SSP",
//t    "attribute : UWTABLE",
//t    "attribute : ARGMEMONLY",
//t    "attribute : STRING '=' STRING",
//t    "attribute : STRING",
//t    "attribute : SYMBOL",
//t    "attribute : READONLY",
//t    "attribute : WRITEONLY",
//t    "attribute : SYMBOL '(' metadata_value_args ')'",
//t    "literal_structure : '{' '}'",
//t    "literal_structure : '{' type_list '}'",
//t    "literal_structure : '<' '{' type_list '}' '>'",
//t    "type_list : type",
//t    "type_list : type_list ',' type",
//t    "return_type : type",
//t    "return_type : VOID",
//t    "type : literal_structure",
//t    "type : INTEGER_TYPE",
//t    "type : HALF",
//t    "type : FLOAT",
//t    "type : DOUBLE",
//t    "type : X86_FP80",
//t    "type : return_type '(' ')'",
//t    "type : return_type '(' function_type_args ')'",
//t    "type : type optional_addrspace '*'",
//t    "type : type optional_addrspace '*' ALIGN INTEGER",
//t    "type : LOCAL_SYMBOL",
//t    "type : PTR optional_addrspace",
//t    "type : '<' INTEGER X type '>'",
//t    "type : '[' INTEGER X type ']'",
//t    "addrspace : ADDRSPACE '(' INTEGER ')'",
//t    "optional_addrspace :",
//t    "optional_addrspace : ADDRSPACE '(' INTEGER ')'",
//t    "function_type_args : function_type_arg",
//t    "function_type_args : function_type_args ',' function_type_arg",
//t    "function_type_arg : type",
//t    "function_type_arg : ELLIPSIS",
//t    "function_definition : define_header GLOBAL_SYMBOL parameters define_tail '{' blocks '}'",
//t    "function_definition : define_header GLOBAL_SYMBOL parameters define_tail metadata_kvs '{' blocks '}'",
//t    "define_tail : function_addr",
//t    "define_tail : function_addr attribute_group_refs",
//t    "define_tail : function_addr attribute_group_refs ALIGN INTEGER",
//t    "define_tail : function_addr attribute_group_refs personality_function",
//t    "define_tail : function_addr attribute_group_refs ALIGN INTEGER personality_function",
//t    "define_tail : function_addr attribute_group_refs SECTION STRING",
//t    "define_tail : attribute_group_refs",
//t    "define_tail : attribute_group_refs ALIGN INTEGER",
//t    "define_tail : attribute_group_refs personality_function",
//t    "define_tail : attribute_group_refs ALIGN INTEGER personality_function",
//t    "define_tail : attribute_group_refs SECTION STRING",
//t    "define_header : DEFINE return_type",
//t    "define_header : DEFINE parameter_attribute return_type",
//t    "define_header : DEFINE define_header_attributes return_type",
//t    "define_header : DEFINE define_header_attributes visibility_style return_type",
//t    "define_header : DEFINE define_header_attributes parameter_attributes return_type",
//t    "define_header_attributes : NOALIAS",
//t    "define_header_attributes : runtime_preemption_specifier",
//t    "define_header_attributes : calling_convention",
//t    "define_header_attributes : linkage",
//t    "define_header_attributes : linkage runtime_preemption_specifier",
//t    "define_header_attributes : linkage runtime_preemption_specifier calling_convention",
//t    "define_header_attributes : linkage calling_convention",
//t    "personality_function : PERSONALITY typed_value",
//t    "declare_head : DECLARE",
//t    "declare_head : DECLARE metadata_kvs",
//t    "function_declaration : declare_head return_type GLOBAL_SYMBOL parameters",
//t    "function_declaration : declare_head return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head NOALIAS return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head parameter_attributes return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head NOALIAS parameter_attributes return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "declare_tail : function_addr",
//t    "declare_tail : function_addr attribute_group_refs",
//t    "declare_tail : attribute_group_refs",
//t    "parameters : '(' parameter_list ')'",
//t    "parameters : '(' ')'",
//t    "parameter_list : parameter",
//t    "parameter_list : parameter_list ',' parameter",
//t    "parameter : type",
//t    "parameter : type LOCAL_SYMBOL",
//t    "parameter : type parameter_attributes",
//t    "parameter : type parameter_attributes LOCAL_SYMBOL",
//t    "parameter : METADATA",
//t    "parameter : ELLIPSIS",
//t    "parameter_attributes : parameter_attribute",
//t    "parameter_attributes : parameter_attributes parameter_attribute",
//t    "parameter_attribute : NONNULL",
//t    "parameter_attribute : NOCAPTURE",
//t    "parameter_attribute : CAPTURES '(' capture_components ')'",
//t    "parameter_attribute : NOUNDEF",
//t    "parameter_attribute : IMMARG",
//t    "parameter_attribute : READONLY",
//t    "parameter_attribute : WRITEONLY",
//t    "parameter_attribute : READNONE",
//t    "parameter_attribute : SIGNEXT",
//t    "parameter_attribute : ZEROEXT",
//t    "parameter_attribute : RETURNED",
//t    "parameter_attribute : SRET",
//t    "parameter_attribute : SRET '(' type ')'",
//t    "parameter_attribute : NOALIAS",
//t    "parameter_attribute : BYVAL",
//t    "parameter_attribute : BYVAL '(' type ')'",
//t    "parameter_attribute : DEREFERENCEABLE '(' INTEGER ')'",
//t    "parameter_attribute : ALIGN INTEGER",
//t    "capture_components : SYMBOL",
//t    "capture_components : capture_components ',' SYMBOL",
//t    "function_addr_type : UNNAMED_ADDR",
//t    "function_addr_type : LOCAL_UNNAMED_ADDR",
//t    "function_addr : function_addr_type optional_addrspace",
//t    "function_addr : function_addr_type optional_addrspace EXTERNALLY_INITIALIZED",
//t    "function_addr : optional_addrspace EXTERNALLY_INITIALIZED",
//t    "function_addr : addrspace",
//t    "runtime_preemption_specifier : DSO_LOCAL",
//t    "runtime_preemption_specifier : DSO_PREEMPTABLE",
//t    "attribute_group_refs : attribute_group_ref",
//t    "attribute_group_refs : attribute_group_refs attribute_group_ref",
//t    "attribute_group_ref : ATTRIBUTE_GROUP_REF",
//t    "icmp_condition : EQ",
//t    "icmp_condition : NE",
//t    "icmp_condition : UGT",
//t    "icmp_condition : UGE",
//t    "icmp_condition : ULT",
//t    "icmp_condition : ULE",
//t    "icmp_condition : SGT",
//t    "icmp_condition : SGE",
//t    "icmp_condition : SLT",
//t    "icmp_condition : SLE",
//t    "fcmp_condition : TRUE",
//t    "fcmp_condition : FALSE",
//t    "fcmp_condition : ORD",
//t    "fcmp_condition : OEQ",
//t    "fcmp_condition : ONE",
//t    "fcmp_condition : OGT",
//t    "fcmp_condition : OGE",
//t    "fcmp_condition : OLT",
//t    "fcmp_condition : OLE",
//t    "fcmp_condition : UNO",
//t    "fcmp_condition : UEQ",
//t    "fcmp_condition : UNE",
//t    "fcmp_condition : UGT",
//t    "fcmp_condition : UGE",
//t    "fcmp_condition : ULT",
//t    "fcmp_condition : ULE",
//t    "global_value : GLOBAL_SYMBOL",
//t    "value : global_value",
//t    "value : nonglobal_value",
//t    "nonglobal_value : constant",
//t    "nonglobal_value : LOCAL_SYMBOL",
//t    "nonglobal_value : INTTOPTR '(' typed_value TO type ')'",
//t    "nonglobal_value : GETELEMENTPTR INBOUNDS '(' type ',' typed_value ',' element_indices ')'",
//t    "nonglobal_value : GETELEMENTPTR INBOUNDS NUW '(' type ',' typed_value ',' element_indices ')'",
//t    "nonglobal_value : GETELEMENTPTR '(' type ',' typed_value ',' element_indices ')'",
//t    "nonglobal_value : BITCAST '(' typed_value TO type ')'",
//t    "nonglobal_value : PTRTOINT '(' typed_value TO type ')'",
//t    "nonglobal_value : '<' typed_values '>'",
//t    "nonglobal_value : '[' typed_values ']'",
//t    "nonglobal_value : '{' typed_values '}'",
//t    "nonglobal_value : '<' '{' typed_values '}' '>'",
//t    "nonglobal_value : ADDRSPACECAST '(' typed_value TO type ')'",
//t    "pointer_value : value",
//t    "constant : NULL",
//t    "constant : FLOAT_LITERAL",
//t    "constant : INTEGER",
//t    "constant : HEX_INTEGER",
//t    "constant : TRUE",
//t    "constant : FALSE",
//t    "constant : UNDEF",
//t    "constant : ZEROINITIALIZER",
//t    "constant : CONSTANT_BYTES",
//t    "label_value : LABEL LOCAL_SYMBOL",
//t    "typed_value : type value",
//t    "typed_value : VOID",
//t    "typed_pointer_value : type pointer_value",
//t    "typed_values : typed_value",
//t    "typed_values : typed_values ',' typed_value",
//t    "typed_constant : type constant",
//t    "element_index : typed_value",
//t    "element_index : INRANGE typed_value",
//t    "element_indices : element_index",
//t    "element_indices : element_indices ',' element_index",
//t    "index : constant",
//t    "indices : index",
//t    "indices : indices ',' index",
//t    "blocks : lblock",
//t    "blocks : blocks lblock",
//t    "lblock : INTEGER ':' block",
//t    "lblock : block",
//t    "block : assignments terminator_assignment",
//t    "block : assignments terminator_assignment metadata_kvs",
//t    "block : terminator_assignment",
//t    "block : terminator_assignment metadata_kvs",
//t    "assignments : assignment",
//t    "assignments : assignments assignment",
//t    "assignment : instruction",
//t    "assignment : instruction metadata_kvs",
//t    "assignment : LOCAL_SYMBOL '=' instruction",
//t    "assignment : LOCAL_SYMBOL '=' instruction metadata_kvs",
//t    "function_pointer : value",
//t    "function_args : '(' function_arg_list ')'",
//t    "function_args : '(' ')'",
//t    "function_arg_list : function_arg",
//t    "function_arg_list : function_arg_list ',' function_arg",
//t    "function_arg : type value",
//t    "function_arg : type parameter_attributes value",
//t    "function_arg : METADATA type metadata_value",
//t    "function_arg : METADATA META_SYMBOL",
//t    "function_arg : METADATA META_SYMBOL '(' ')'",
//t    "function_arg : METADATA META_SYMBOL '(' metadata_value_args ')'",
//t    "metadata_value : constant",
//t    "metadata_value : GLOBAL_SYMBOL",
//t    "metadata_value : LOCAL_SYMBOL",
//t    "metadata_value : SYMBOL",
//t    "metadata_value : INTTOPTR '(' typed_value TO type ')'",
//t    "metadata_value : GETELEMENTPTR INBOUNDS '(' type ',' typed_value ',' element_indices ')'",
//t    "metadata_value : GETELEMENTPTR INBOUNDS NUW '(' type ',' typed_value ',' element_indices ')'",
//t    "metadata_value : BITCAST '(' typed_value TO type ')'",
//t    "metadata_value : PTRTOINT '(' typed_value TO type ')'",
//t    "metadata_value_args : metadata_value_arg",
//t    "metadata_value_args : metadata_value_args ',' metadata_value_arg",
//t    "metadata_value_arg : constant",
//t    "metadata_value_arg : SYMBOL",
//t    "metadata_value_arg : type LOCAL_SYMBOL",
//t    "metadata_value_arg : type SYMBOL",
//t    "metadata_value_arg : type UNDEF",
//t    "phi_vals : phi_val",
//t    "phi_vals : phi_vals ',' phi_val",
//t    "phi_val : '[' value ',' value ']'",
//t    "switch_cases : switch_case",
//t    "switch_cases : switch_cases switch_case",
//t    "switch_case : typed_constant ',' label_value",
//t    "wrappings : wrapping",
//t    "wrappings : wrappings wrapping",
//t    "wrapping : NUW",
//t    "wrapping : NSW",
//t    "calling_convention : FASTCC",
//t    "atomic_constraint : SEQ_CST",
//t    "inline_assembly : ASM SIDEEFFECT STRING ',' STRING",
//t    "terminator_assignment : terminator_instruction",
//t    "terminator_assignment : LOCAL_SYMBOL '=' invoke_instruction",
//t    "terminator_instruction : BR label_value",
//t    "terminator_instruction : BR INTEGER_TYPE value ',' label_value ',' label_value",
//t    "terminator_instruction : RESUME typed_value",
//t    "terminator_instruction : RET typed_value",
//t    "terminator_instruction : SWITCH typed_value ',' label_value '[' switch_cases ']'",
//t    "terminator_instruction : UNREACHABLE",
//t    "terminator_instruction : invoke_instruction",
//t    "invoke_instruction : INVOKE return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "invoke_instruction : INVOKE parameter_attributes return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "invoke_instruction : INVOKE calling_convention return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "invoke_instruction : INVOKE calling_convention parameter_attributes return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "optional_fast :",
//t    "optional_fast : FAST",
//t    "instruction : ADD type value ',' value",
//t    "instruction : ADD wrappings type value ',' value",
//t    "instruction : ATOMICRMW ADD type value ',' type value SEQ_CST ',' ALIGN INTEGER",
//t    "instruction : ALLOCA type ',' ALIGN INTEGER",
//t    "instruction : ALLOCA type ',' typed_value ',' ALIGN INTEGER",
//t    "instruction : AND type value ',' value",
//t    "instruction : ASHR type value ',' value",
//t    "instruction : ASHR EXACT type value ',' value",
//t    "instruction : BITCAST typed_value TO type",
//t    "instruction : CALL return_type function_pointer function_args",
//t    "instruction : CALL return_type inline_assembly function_args attribute_group_refs",
//t    "instruction : CALL calling_convention return_type function_pointer function_args",
//t    "instruction : CALL calling_convention return_type function_pointer function_args attribute_group_refs",
//t    "instruction : CALL calling_convention parameter_attribute return_type function_pointer function_args",
//t    "instruction : CALL calling_convention parameter_attribute return_type function_pointer function_args attribute_group_refs",
//t    "instruction : CALL return_type function_pointer function_args attribute_group_refs",
//t    "instruction : CALL parameter_attribute return_type function_pointer function_args",
//t    "instruction : CALL parameter_attribute return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL FAST return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL parameter_attribute return_type function_pointer function_args",
//t    "instruction : TAIL CALL parameter_attribute return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL calling_convention parameter_attributes return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention parameter_attributes return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type inline_assembly function_args attribute_group_refs",
//t    "instruction : EXTRACTELEMENT typed_value ',' typed_value",
//t    "instruction : EXTRACTVALUE typed_value ',' indices",
//t    "instruction : FADD optional_fast type value ',' value",
//t    "instruction : FCMP optional_fast fcmp_condition type value ',' value",
//t    "instruction : FDIV optional_fast type value ',' value",
//t    "instruction : FENCE atomic_constraint",
//t    "instruction : FMUL optional_fast type value ',' value",
//t    "instruction : FPEXT typed_value TO type",
//t    "instruction : FPTOUI typed_value TO type",
//t    "instruction : FPTOSI typed_value TO type",
//t    "instruction : FPTRUNC typed_value TO type",
//t    "instruction : FSUB optional_fast type value ',' value",
//t    "instruction : GETELEMENTPTR type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR INBOUNDS type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR INBOUNDS NUW type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR NUW type ',' typed_value ',' element_indices",
//t    "instruction : ICMP icmp_condition type value ',' value",
//t    "instruction : INSERTELEMENT typed_value ',' typed_value ',' typed_value",
//t    "instruction : INSERTVALUE typed_value ',' typed_value ',' indices",
//t    "instruction : INTTOPTR typed_value TO type",
//t    "instruction : LANDINGPAD type CLEANUP",
//t    "instruction : LANDINGPAD type CATCH typed_value",
//t    "instruction : LOAD type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD ATOMIC type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD VOLATILE type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD ATOMIC VOLATILE type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD ATOMIC VOLATILE type ',' typed_pointer_value MONOTONIC ',' ALIGN INTEGER",
//t    "instruction : LSHR type value ',' value",
//t    "instruction : LSHR EXACT type value ',' value",
//t    "instruction : OR type value ',' value",
//t    "instruction : MUL type value ',' value",
//t    "instruction : MUL wrappings type value ',' value",
//t    "instruction : PHI type phi_vals",
//t    "instruction : PTRTOINT typed_value TO type",
//t    "instruction : SDIV type value ',' value",
//t    "instruction : SDIV EXACT type value ',' value",
//t    "instruction : SELECT optional_fast type value ',' typed_value ',' typed_value",
//t    "instruction : SEXT typed_value TO type",
//t    "instruction : SHL type value ',' value",
//t    "instruction : SHL wrappings type value ',' value",
//t    "instruction : SHUFFLEVECTOR typed_value ',' typed_value ',' typed_value",
//t    "instruction : SITOFP typed_value TO type",
//t    "instruction : SREM type value ',' value",
//t    "instruction : STORE typed_value ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : STORE VOLATILE typed_value ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : SUB type value ',' value",
//t    "instruction : SUB wrappings type value ',' value",
//t    "instruction : ATOMICRMW SUB type value ',' type value SEQ_CST ',' ALIGN INTEGER",
//t    "instruction : TRUNC typed_value TO type",
//t    "instruction : UDIV type value ',' value",
//t    "instruction : UITOFP typed_value TO type",
//t    "instruction : UREM type value ',' value",
//t    "instruction : XOR type value ',' value",
//t    "instruction : ZEXT typed_value TO type",
//t  };
//t public static string getRule (int index) {
//t    return yyRule [index];
//t }
//t}
  protected static readonly string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,"'!'",null,null,null,null,null,
    null,"'('","')'","'*'",null,"','",null,null,null,null,null,null,null,
    null,null,null,null,null,null,"':'",null,"'<'","'='","'>'",null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,"'['",
    null,"']'",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,"'{'","'|'","'}'",null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,
    "INTEGER","HEX_INTEGER","FLOAT_LITERAL","STRING","TRUE","FALSE",
    "UNDEF","VOID","NULL","LABEL","X","SOURCE_FILENAME","TARGET",
    "DATALAYOUT","TRIPLE","GLOBAL_SYMBOL","LOCAL_SYMBOL","META_SYMBOL",
    "META_SYMBOL_DEF","SYMBOL","DISTINCT","METADATA","CONSTANT_BYTES",
    "SECTION","TYPE","HALF","FLOAT","DOUBLE","X86_FP80","INTEGER_TYPE",
    "ZEROINITIALIZER","OPAQUE","PTR","DEFINE","DECLARE","UNNAMED_ADDR",
    "LOCAL_UNNAMED_ADDR","NOALIAS","ELLIPSIS","GLOBAL","CONSTANT",
    "PRIVATE","INTERNAL","EXTERNAL","LINKONCE","LINKONCE_ODR","WEAK",
    "WEAK_ODR","APPENDING","COMMON","FASTCC","SIGNEXT","ZEROEXT",
    "VOLATILE","RETURNED","DEREFERENCEABLE","AVAILABLE_EXTERNALLY",
    "PERSONALITY","SRET","CLEANUP","EXTERNALLY_INITIALIZED","NONNULL",
    "NOCAPTURE","WRITEONLY","READONLY","READNONE","HIDDEN","BYVAL",
    "ATTRIBUTE_GROUP_REF","ATTRIBUTES","NORECURSE","NOUNWIND","UNWIND",
    "SPECULATABLE","SSP","UWTABLE","ARGMEMONLY","SEQ_CST","DSO_LOCAL",
    "DSO_PREEMPTABLE","RET","BR","SWITCH","INDIRECTBR","INVOKE","RESUME",
    "CATCHSWITCH","CATCHRET","CLEANUPRET","UNREACHABLE","FNEG","ADD",
    "NUW","NSW","FADD","SUB","FSUB","MUL","FMUL","UDIV","SDIV","FDIV",
    "UREM","SREM","FREM","SHL","LSHR","EXACT","ASHR","AND","OR","XOR",
    "EXTRACTELEMENT","INSERTELEMENT","SHUFFLEVECTOR","EXTRACTVALUE",
    "INSERTVALUE","ALLOCA","LOAD","STORE","FENCE","CMPXCHG","ATOMICRMW",
    "GETELEMENTPTR","ALIGN","INBOUNDS","INRANGE","ADDRSPACE","TRUNC",
    "ZEXT","SEXT","FPTRUNC","FPEXT","TO","FPTOUI","FPTOSI","UITOFP",
    "SITOFP","PTRTOINT","INTTOPTR","BITCAST","ADDRSPACECAST","ICMP","EQ",
    "NE","UGT","UGE","ULT","ULE","SGT","SGE","SLT","SLE","FCMP","OEQ",
    "OGT","OGE","OLT","OLE","ONE","ORD","UEQ","UNE","UNO","FAST","PHI",
    "SELECT","CALL","TAIL","VA_ARG","ASM","SIDEEFFECT","LANDINGPAD",
    "CATCH","CATCHPAD","CLEANUPPAD","NOUNDEF","IMMARG","CAPTURES",
    "ATOMIC","MONOTONIC",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
  public static string yyname (int token) {
    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
    string name;
    if ((name = yyNames[token]) != null) return name;
    return "[unknown]";
  }

  //int yyExpectingState;
  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected int [] yyExpectingTokens (int state){
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];
    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    int [] result = new int [len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = token;
    return result;
  }
  protected string[] yyExpecting (int state) {
    int [] tokens = yyExpectingTokens (state);
    string [] result = new string[tokens.Length];
    for (int n = 0; n < tokens.Length;  n++)
      result[n] = yyNames[tokens [n]];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
//t    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

	static int[] global_yyStates;
	static object[] global_yyVals;
	protected bool use_global_stacks;
	object[] yyVals;					// value stack
	object yyVal;						// value stack ptr
	int yyToken;						// current input
	int yyTop;

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;		// initial size
    int yyState = 0;                   // state stack ptr
    int [] yyStates;               	// state stack 
    yyVal = null;
    yyToken = -1;
    int yyErrorFlag = 0;				// #tks to shift
	if (use_global_stacks && global_yyStates != null) {
		yyVals = global_yyVals;
		yyStates = global_yyStates;
   } else {
		yyVals = new object [yyMax];
		yyStates = new int [yyMax];
		if (use_global_stacks) {
			global_yyVals = yyVals;
			global_yyStates = yyStates;
		}
	}

    /*yyLoop:*/ for (yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        global::System.Array.Resize (ref yyStates, yyStates.Length+yyMax);
        global::System.Array.Resize (ref yyVals, yyVals.Length+yyMax);
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
//t      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ while (true) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
//t            if (debug != null)
//t              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              //yyExpectingState = yyState;
              SyntaxError(yyname (yyToken), String.Join(", ", yyExpecting(yyState)));
//t              if (debug != null) debug.error("syntax error");
              if (yyToken == 0 /*eof*/ || yyToken == eof_token) throw new yyParser.yyUnexpectedEof ();
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += Token.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == Token.yyErrorCode) {
//t                  if (debug != null)
//t                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
//t                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
//t              if (debug != null) debug.reject();
              throw new yyParser.yyException("Irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
//t                if (debug != null) debug.reject();
                throw new yyParser.yyException("Irrecoverable syntax error at end-of-file");
              }
//t              if (debug != null)
//t                debug.discard(yyState, yyToken, yyname(yyToken),
//t  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
//t        if (debug != null)
//t          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyV > yyTop ? null : yyVals[yyV]; // yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
case 4:
#line 64 "Iril/IR/IR.jay"
  {
        module.SourceFilename = (string)yyVals[0+yyTop];
    }
  break;
case 5:
#line 68 "Iril/IR/IR.jay"
  {
        module.TargetDatalayout = (string)yyVals[0+yyTop];
    }
  break;
case 6:
#line 72 "Iril/IR/IR.jay"
  {
        module.TargetTriple = (string)yyVals[0+yyTop];
    }
  break;
case 7:
#line 76 "Iril/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = (StructureType)yyVals[0+yyTop];
    }
  break;
case 8:
#line 80 "Iril/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = OpaqueStructureType.Opaque;
    }
  break;
case 9:
  case_9();
  break;
case 10:
  case_10();
  break;
case 11:
  case_11();
  break;
case 14:
#line 101 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-4+yyTop]] = new List<object> (0);
    }
  break;
case 15:
#line 105 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 16:
  case_16();
  break;
case 17:
#line 114 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 18:
  case_18();
  break;
case 19:
#line 126 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 20:
#line 130 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-3+yyTop], isConstant: (bool)yyVals[-3+yyTop]);
    }
  break;
case 21:
#line 134 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 22:
#line 138 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 23:
#line 142 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-12+yyTop], (LType)yyVals[-7+yyTop], (Value)yyVals[-6+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-8+yyTop]);
    }
  break;
case 24:
#line 146 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 25:
#line 150 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 26:
#line 154 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 27:
#line 158 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 28:
#line 162 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-7+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 29:
#line 166 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-6+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: (bool)yyVals[-4+yyTop], isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 30:
#line 170 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: (bool)yyVals[-8+yyTop], isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 31:
#line 174 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 32:
#line 178 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 33:
#line 182 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-8+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 34:
#line 186 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 35:
#line 190 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 36:
#line 194 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-7+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-5+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 37:
#line 198 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-4+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-2+yyTop], isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 38:
#line 202 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 39:
#line 203 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 40:
#line 207 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 41:
#line 208 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 42:
#line 209 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 43:
#line 210 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 44:
#line 211 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 45:
#line 212 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 46:
#line 213 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 47:
#line 214 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 48:
#line 215 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 49:
#line 219 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 50:
#line 223 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 51:
  case_51();
  break;
case 52:
  case_52();
  break;
case 53:
#line 240 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 54:
#line 241 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 55:
#line 242 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 56:
#line 246 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-5+yyTop], yyVals[-3+yyTop]);
    }
  break;
case 57:
#line 250 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-4+yyTop], yyVals[-2+yyTop]);
    }
  break;
case 68:
#line 279 "Iril/IR/IR.jay"
  {
        yyVal = NewSyms (yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 69:
#line 283 "Iril/IR/IR.jay"
  {
        yyVal = SymsAdd (yyVals[-2+yyTop], yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 70:
#line 290 "Iril/IR/IR.jay"
  {
        yyVal = NewList (yyVals[0+yyTop]);
    }
  break;
case 71:
#line 294 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 72:
#line 298 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 73:
#line 302 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 74:
#line 306 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 93:
#line 340 "Iril/IR/IR.jay"
  {
        yyVal = LiteralStructureType.Empty;
    }
  break;
case 94:
#line 344 "Iril/IR/IR.jay"
  {
        yyVal = new LiteralStructureType (false, (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 95:
#line 348 "Iril/IR/IR.jay"
  {
        yyVal = new PackedStructureType ((List<LType>)yyVals[-2+yyTop]);
    }
  break;
case 96:
#line 355 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 97:
#line 359 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 99:
#line 364 "Iril/IR/IR.jay"
  { yyVal = VoidType.Void; }
  break;
case 102:
#line 370 "Iril/IR/IR.jay"
  { yyVal = FloatType.Half; }
  break;
case 103:
#line 371 "Iril/IR/IR.jay"
  { yyVal = FloatType.Float; }
  break;
case 104:
#line 372 "Iril/IR/IR.jay"
  { yyVal = FloatType.Double; }
  break;
case 105:
#line 373 "Iril/IR/IR.jay"
  { yyVal = FloatType.X86_FP80; }
  break;
case 106:
#line 377 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-2+yyTop], Enumerable.Empty<LType>());
    }
  break;
case 107:
#line 381 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-3+yyTop], (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 108:
#line 385 "Iril/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-2+yyTop], 0);
    }
  break;
case 109:
#line 389 "Iril/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-4+yyTop], 0);
    }
  break;
case 110:
#line 393 "Iril/IR/IR.jay"
  {
        yyVal = new NamedType ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 111:
#line 397 "Iril/IR/IR.jay"
  {
        yyVal = PointerType.OpaquePointer;
    }
  break;
case 112:
#line 401 "Iril/IR/IR.jay"
  {
        yyVal = new VectorType ((int)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 113:
#line 405 "Iril/IR/IR.jay"
  {
        yyVal = new ArrayType ((long)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 117:
#line 421 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 118:
#line 425 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 120:
#line 433 "Iril/IR/IR.jay"
  {
        yyVal = VarArgsType.VarArgs;
    }
  break;
case 121:
  case_121();
  break;
case 122:
  case_122();
  break;
case 134:
#line 467 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create ((object)true, yyVals[0+yyTop]);
    }
  break;
case 135:
#line 471 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create ((object)true, yyVals[0+yyTop]);
    }
  break;
case 136:
#line 475 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 137:
#line 479 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 138:
#line 483 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 139:
#line 490 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 140:
#line 494 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 141:
#line 498 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 146:
#line 509 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 149:
#line 521 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-2+yyTop], (GlobalSymbol)yyVals[-1+yyTop], (IEnumerable<Parameter>)yyVals[0+yyTop]);
    }
  break;
case 150:
#line 525 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 151:
#line 529 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 152:
#line 533 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 153:
#line 537 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 157:
#line 547 "Iril/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 158:
#line 548 "Iril/IR/IR.jay"
  { yyVal = Enumerable.Empty<Parameter> (); }
  break;
case 159:
#line 555 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Parameter)yyVals[0+yyTop]);
    }
  break;
case 160:
#line 559 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Parameter)yyVals[0+yyTop]);
    }
  break;
case 161:
#line 566 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[0+yyTop]);
    }
  break;
case 162:
#line 570 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter ((LocalSymbol)yyVals[0+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 163:
#line 574 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[-1+yyTop]);
    }
  break;
case 164:
#line 578 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter ((LocalSymbol)yyVals[0+yyTop], (LType)yyVals[-2+yyTop]);
    }
  break;
case 165:
#line 582 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, IntegerType.I32);
    }
  break;
case 166:
#line 586 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, VarArgsType.VarArgs);
    }
  break;
case 168:
#line 594 "Iril/IR/IR.jay"
  {
        yyVal = ((ParameterAttributes)yyVals[-1+yyTop]) | ((ParameterAttributes)yyVals[0+yyTop]);
    }
  break;
case 169:
#line 598 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NonNull; }
  break;
case 170:
#line 599 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 171:
#line 600 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 172:
#line 601 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoUndef; }
  break;
case 173:
#line 602 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ImmediateArgument; }
  break;
case 174:
#line 603 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadOnly; }
  break;
case 175:
#line 604 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.WriteOnly; }
  break;
case 176:
#line 605 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadNone; }
  break;
case 177:
#line 606 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.SignExtend; }
  break;
case 178:
#line 607 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ZeroExtend; }
  break;
case 179:
#line 608 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Returned; }
  break;
case 180:
#line 609 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.StructureReturn; }
  break;
case 181:
#line 610 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.StructureReturn; }
  break;
case 182:
#line 611 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoAlias; }
  break;
case 183:
#line 612 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Byval; }
  break;
case 184:
#line 613 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Byval; }
  break;
case 185:
#line 617 "Iril/IR/IR.jay"
  {
        yyVal = ParameterAttributes.Dereferenceable;
    }
  break;
case 186:
#line 621 "Iril/IR/IR.jay"
  {
        yyVal = ParameterAttributes.Align8;
    }
  break;
case 200:
#line 656 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.Equal; }
  break;
case 201:
#line 657 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.NotEqual; }
  break;
case 202:
#line 658 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThan; }
  break;
case 203:
#line 659 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThanOrEqual; }
  break;
case 204:
#line 660 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThan; }
  break;
case 205:
#line 661 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThanOrEqual; }
  break;
case 206:
#line 662 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThan; }
  break;
case 207:
#line 663 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThanOrEqual; }
  break;
case 208:
#line 664 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThan; }
  break;
case 209:
#line 665 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThanOrEqual; }
  break;
case 210:
#line 669 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.True; }
  break;
case 211:
#line 670 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.False; }
  break;
case 212:
#line 671 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.Ordered; }
  break;
case 213:
#line 672 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedEqual; }
  break;
case 214:
#line 673 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedNotEqual; }
  break;
case 215:
#line 674 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThan; }
  break;
case 216:
#line 675 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThanOrEqual; }
  break;
case 217:
#line 676 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThan; }
  break;
case 218:
#line 677 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThanOrEqual; }
  break;
case 219:
#line 678 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.Unordered; }
  break;
case 220:
#line 679 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedEqual; }
  break;
case 221:
#line 680 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedNotEqual; }
  break;
case 222:
#line 681 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThan; }
  break;
case 223:
#line 682 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThanOrEqual; }
  break;
case 224:
#line 683 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThan; }
  break;
case 225:
#line 684 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThanOrEqual; }
  break;
case 226:
#line 688 "Iril/IR/IR.jay"
  { yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]); }
  break;
case 230:
#line 698 "Iril/IR/IR.jay"
  { yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]); }
  break;
case 231:
#line 702 "Iril/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 232:
#line 706 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 233:
#line 710 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 234:
#line 714 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 235:
#line 718 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 236:
#line 722 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 237:
#line 726 "Iril/IR/IR.jay"
  {
        yyVal = new VectorConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 238:
#line 730 "Iril/IR/IR.jay"
  {
        yyVal = new ArrayConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 239:
#line 734 "Iril/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 240:
#line 738 "Iril/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-2+yyTop]);
    }
  break;
case 241:
#line 742 "Iril/IR/IR.jay"
  {
        yyVal = new AddrSpaceCastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 243:
#line 750 "Iril/IR/IR.jay"
  { yyVal = NullConstant.Null; }
  break;
case 244:
#line 751 "Iril/IR/IR.jay"
  { yyVal = new FloatConstant ((double)yyVals[0+yyTop]); }
  break;
case 245:
#line 752 "Iril/IR/IR.jay"
  { yyVal = new IntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 246:
#line 753 "Iril/IR/IR.jay"
  { yyVal = new HexIntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 247:
#line 754 "Iril/IR/IR.jay"
  { yyVal = BooleanConstant.True; }
  break;
case 248:
#line 755 "Iril/IR/IR.jay"
  { yyVal = BooleanConstant.False; }
  break;
case 249:
#line 756 "Iril/IR/IR.jay"
  { yyVal = UndefinedConstant.Undefined; }
  break;
case 250:
#line 757 "Iril/IR/IR.jay"
  { yyVal = ZeroConstant.Zero; }
  break;
case 251:
#line 758 "Iril/IR/IR.jay"
  { yyVal = new BytesConstant ((Symbol)yyVals[0+yyTop]); }
  break;
case 252:
#line 765 "Iril/IR/IR.jay"
  {
        yyVal = new LabelValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 253:
#line 772 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 254:
#line 776 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue (VoidType.Void, VoidValue.Void);
    }
  break;
case 255:
#line 783 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 256:
#line 790 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 257:
#line 794 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 258:
#line 801 "Iril/IR/IR.jay"
  {
        yyVal = new TypedConstant ((LType)yyVals[-1+yyTop], (Constant)yyVals[0+yyTop]);
    }
  break;
case 260:
#line 809 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 261:
#line 816 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 262:
#line 820 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 264:
#line 831 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Value)yyVals[0+yyTop]);
    }
  break;
case 265:
#line 835 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 266:
#line 842 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Block)yyVals[0+yyTop]);
    }
  break;
case 267:
#line 846 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Block)yyVals[0+yyTop]);
    }
  break;
case 268:
#line 853 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 269:
#line 857 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 270:
#line 864 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 271:
#line 868 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-2+yyTop], (Assignment)yyVals[-1+yyTop]);
    }
  break;
case 272:
#line 872 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (Assignment)yyVals[0+yyTop]);
    }
  break;
case 273:
#line 876 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (Assignment)yyVals[-1+yyTop]);
    }
  break;
case 274:
#line 883 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Assignment)yyVals[0+yyTop]);
    }
  break;
case 275:
#line 887 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 276:
#line 894 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[0+yyTop]);
    }
  break;
case 277:
#line 898 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[0+yyTop]);
    }
  break;
case 278:
#line 902 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 279:
#line 906 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-3+yyTop], (Instruction)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[0+yyTop]);
    }
  break;
case 281:
#line 914 "Iril/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 282:
#line 915 "Iril/IR/IR.jay"
  { yyVal = Enumerable.Empty<Argument> (); }
  break;
case 283:
#line 922 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Argument)yyVals[0+yyTop]);
    }
  break;
case 284:
#line 926 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Argument)yyVals[0+yyTop]);
    }
  break;
case 285:
#line 933 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 286:
#line 937 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], ParameterAttributes.NonNull);
    }
  break;
case 287:
#line 941 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 288:
#line 945 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[0+yyTop]), (ParameterAttributes)0);
    }
  break;
case 289:
#line 949 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-2+yyTop]), (ParameterAttributes)0);
    }
  break;
case 290:
#line 953 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-3+yyTop]), (ParameterAttributes)0);
    }
  break;
case 292:
#line 961 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]);
    }
  break;
case 293:
#line 965 "Iril/IR/IR.jay"
  {
        yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 294:
#line 969 "Iril/IR/IR.jay"
  {
        yyVal = new SymbolValue ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 295:
#line 973 "Iril/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 296:
#line 977 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 297:
#line 981 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 298:
#line 985 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 299:
#line 989 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 307:
#line 1009 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((PhiValue)yyVals[0+yyTop]);
    }
  break;
case 308:
#line 1013 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (PhiValue)yyVals[0+yyTop]);
    }
  break;
case 309:
#line 1019 "Iril/IR/IR.jay"
  {
        yyVal = new PhiValue ((Value)yyVals[-3+yyTop], (Value)yyVals[-1+yyTop]);
    }
  break;
case 310:
#line 1026 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 311:
#line 1030 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 312:
#line 1037 "Iril/IR/IR.jay"
  {
        yyVal = new SwitchCase ((TypedConstant)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 318:
#line 1055 "Iril/IR/IR.jay"
  { yyVal = AtomicConstraint.SequentiallyConsistent; }
  break;
case 319:
#line 1062 "Iril/IR/IR.jay"
  {
        yyVal = new InlineAssemblyValue ((string)yyVals[-2+yyTop], (string)yyVals[0+yyTop]);
    }
  break;
case 320:
#line 1069 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment (LocalSymbol.None, (Instruction)yyVals[0+yyTop]);
    }
  break;
case 321:
#line 1073 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 322:
#line 1080 "Iril/IR/IR.jay"
  {
        yyVal = new UnconditionalBrInstruction ((LabelValue)yyVals[0+yyTop]);
    }
  break;
case 323:
#line 1084 "Iril/IR/IR.jay"
  {
        yyVal = new ConditionalBrInstruction ((Value)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 324:
#line 1088 "Iril/IR/IR.jay"
  {
        yyVal = new ResumeInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 325:
#line 1092 "Iril/IR/IR.jay"
  {
        yyVal = new RetInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 326:
#line 1096 "Iril/IR/IR.jay"
  {
        yyVal = new SwitchInstruction ((TypedValue)yyVals[-5+yyTop], (LabelValue)yyVals[-3+yyTop], (List<SwitchCase>)yyVals[-1+yyTop]);
    }
  break;
case 327:
#line 1100 "Iril/IR/IR.jay"
  {
        yyVal = UnreachableInstruction.Unreachable;
    }
  break;
case 329:
#line 1108 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 330:
#line 1112 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 331:
#line 1116 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 332:
#line 1120 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 333:
#line 1127 "Iril/IR/IR.jay"
  {
        yyVal = false;
    }
  break;
case 334:
#line 1131 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 335:
#line 1138 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 336:
#line 1142 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 337:
#line 1146 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-8+yyTop], (Value)yyVals[-7+yyTop], (Value)yyVals[-4+yyTop], isAtomic: true);
    }
  break;
case 338:
#line 1150 "Iril/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-3+yyTop], (int)(BigInteger)yyVals[0+yyTop], numElements: (TypedValue)null);
    }
  break;
case 339:
#line 1154 "Iril/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-5+yyTop], (int)(BigInteger)yyVals[0+yyTop], numElements: (TypedValue)yyVals[-3+yyTop]);
    }
  break;
case 340:
#line 1158 "Iril/IR/IR.jay"
  {
        yyVal = new AndInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 341:
#line 1162 "Iril/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 342:
#line 1166 "Iril/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 343:
#line 1170 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 344:
#line 1174 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 345:
#line 1178 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 346:
#line 1182 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 347:
#line 1186 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 348:
#line 1190 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 349:
#line 1194 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 350:
#line 1198 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 351:
#line 1202 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 352:
#line 1206 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 353:
#line 1210 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 354:
#line 1214 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 355:
#line 1218 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 356:
#line 1222 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 357:
#line 1226 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 358:
#line 1230 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 359:
#line 1234 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 360:
#line 1238 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 361:
#line 1242 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 362:
#line 1246 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 363:
#line 1250 "Iril/IR/IR.jay"
  {
        yyVal = new ExtractElementInstruction ((TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 364:
#line 1254 "Iril/IR/IR.jay"
  {
        yyVal = new ExtractValueInstruction ((TypedValue)yyVals[-2+yyTop], (List<Value>)yyVals[0+yyTop]);
    }
  break;
case 365:
#line 1258 "Iril/IR/IR.jay"
  {
        yyVal = new FaddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 366:
#line 1262 "Iril/IR/IR.jay"
  {
        yyVal = new FcmpInstruction ((FcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 367:
#line 1266 "Iril/IR/IR.jay"
  {
        yyVal = new FdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 368:
#line 1270 "Iril/IR/IR.jay"
  {
        yyVal = new FenceInstruction ((AtomicConstraint)yyVals[0+yyTop]);
    }
  break;
case 369:
#line 1274 "Iril/IR/IR.jay"
  {
        yyVal = new FmulInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 370:
#line 1278 "Iril/IR/IR.jay"
  {
        yyVal = new FpextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 371:
#line 1282 "Iril/IR/IR.jay"
  {
        yyVal = new FptouiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 372:
#line 1286 "Iril/IR/IR.jay"
  {
        yyVal = new FptosiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 373:
#line 1290 "Iril/IR/IR.jay"
  {
        yyVal = new FptruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 374:
#line 1294 "Iril/IR/IR.jay"
  {
        yyVal = new FsubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 375:
#line 1298 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 376:
#line 1302 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 377:
#line 1306 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 378:
#line 1310 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 379:
#line 1314 "Iril/IR/IR.jay"
  {
        yyVal = new IcmpInstruction ((IcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 380:
#line 1318 "Iril/IR/IR.jay"
  {
        yyVal = new InsertElementInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 381:
#line 1322 "Iril/IR/IR.jay"
  {
        yyVal = new InsertValueInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<Value>)yyVals[0+yyTop]);
    }
  break;
case 382:
#line 1326 "Iril/IR/IR.jay"
  {
        yyVal = new InttoptrInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 383:
#line 1330 "Iril/IR/IR.jay"
  {
        yyVal = new LandingPadInstruction ((LType)yyVals[-1+yyTop]);
    }
  break;
case 384:
#line 1334 "Iril/IR/IR.jay"
  {
        yyVal = new LandingPadInstruction ((LType)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 385:
#line 1338 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false, isAtomic: false);
    }
  break;
case 386:
#line 1342 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false, isAtomic: true);
    }
  break;
case 387:
#line 1346 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true, isAtomic: false);
    }
  break;
case 388:
#line 1350 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true, isAtomic: true);
    }
  break;
case 389:
#line 1354 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-6+yyTop], (TypedValue)yyVals[-4+yyTop], isVolatile: true, isAtomic: true);
    }
  break;
case 390:
#line 1358 "Iril/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 391:
#line 1362 "Iril/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 392:
#line 1366 "Iril/IR/IR.jay"
  {
        yyVal = new OrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 393:
#line 1370 "Iril/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 394:
#line 1374 "Iril/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 395:
#line 1378 "Iril/IR/IR.jay"
  {
        yyVal = new PhiInstruction ((LType)yyVals[-1+yyTop], (List<PhiValue>)yyVals[0+yyTop]);
    }
  break;
case 396:
#line 1382 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 397:
#line 1386 "Iril/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 398:
#line 1390 "Iril/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 399:
#line 1394 "Iril/IR/IR.jay"
  {
        yyVal = new SelectInstruction ((LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 400:
#line 1398 "Iril/IR/IR.jay"
  {
        yyVal = new SextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 401:
#line 1402 "Iril/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 402:
#line 1406 "Iril/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 403:
#line 1410 "Iril/IR/IR.jay"
  {
        yyVal = new ShuffleVectorInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 404:
#line 1414 "Iril/IR/IR.jay"
  {
        yyVal = new SitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 405:
#line 1418 "Iril/IR/IR.jay"
  {
        yyVal = new SremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 406:
#line 1422 "Iril/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: false);
    }
  break;
case 407:
#line 1426 "Iril/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: true);
    }
  break;
case 408:
#line 1430 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 409:
#line 1434 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 410:
#line 1438 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-8+yyTop], (Value)yyVals[-7+yyTop], (Value)yyVals[-4+yyTop], isAtomic: true);
    }
  break;
case 411:
#line 1442 "Iril/IR/IR.jay"
  {
        yyVal = new TruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 412:
#line 1446 "Iril/IR/IR.jay"
  {
        yyVal = new UdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 413:
#line 1450 "Iril/IR/IR.jay"
  {
        yyVal = new UitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 414:
#line 1454 "Iril/IR/IR.jay"
  {
        yyVal = new UremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 415:
#line 1458 "Iril/IR/IR.jay"
  {
        yyVal = new XorInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 416:
#line 1462 "Iril/IR/IR.jay"
  {
        yyVal = new ZextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
#line default
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
//t          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
//t            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
//t        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: ;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: ;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

/*
 All more than 3 lines long rules are wrapped into a method
*/
void case_9()
#line 82 "Iril/IR/IR.jay"
{
        var f = (FunctionDefinition)yyVals[0+yyTop];
        module.FunctionDefinitions[f.Symbol] = f;
    }

void case_10()
#line 87 "Iril/IR/IR.jay"
{
        var f = (FunctionDeclaration)yyVals[0+yyTop];
        module.FunctionDeclarations[f.Symbol] = f;
    }

void case_11()
#line 92 "Iril/IR/IR.jay"
{
        var g = (GlobalVariable)yyVals[0+yyTop];
        module.AddGlobalVariable(g);
    }

void case_16()
#line 107 "Iril/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = m;
    }

void case_18()
#line 116 "Iril/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = m;
    }

void case_51()
#line 228 "Iril/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = NewSyms (t.Item1, t.Item2);
    }

void case_52()
#line 233 "Iril/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = SymsAdd (yyVals[-2+yyTop], t.Item1, t.Item2);
    }

void case_121()
#line 438 "Iril/IR/IR.jay"
{
        var h = (Tuple<object, object>)yyVals[-6+yyTop];
        yyVal = new FunctionDefinition ((LType)h.Item2, (GlobalSymbol)yyVals[-5+yyTop], (IEnumerable<Parameter>)yyVals[-4+yyTop], (List<Block>)yyVals[-1+yyTop], isExternal: (bool)h.Item1);
    }

void case_122()
#line 443 "Iril/IR/IR.jay"
{
        var h = (Tuple<object, object>)yyVals[-7+yyTop];
        yyVal = new FunctionDefinition ((LType)h.Item2, (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop], isExternal: (bool)h.Item1, (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }

#line default
   static readonly short [] yyLhs  = {              -1,
    0,    1,    1,    2,    2,    2,    2,    2,    2,    2,
    2,    2,    2,    2,    2,    2,    2,    2,    6,    6,
    6,    6,    6,    6,    6,    6,    6,    6,    6,    6,
    6,    6,    6,    6,    6,    6,    6,   11,   11,   10,
   10,   10,   10,   10,   10,   10,   10,   10,   17,   14,
    9,    9,   18,   18,   18,   18,   18,   19,   22,   22,
   23,   24,   24,   24,   24,   24,   24,   16,   16,    8,
    8,    8,    8,    8,   26,   26,   26,    7,    7,   28,
   28,   28,   28,   28,   28,   28,   28,   28,   28,   28,
   28,   28,    3,    3,    3,   29,   29,   30,   30,   12,
   12,   12,   12,   12,   12,   12,   12,   12,   12,   12,
   12,   12,   12,   33,   32,   32,   31,   31,   34,   34,
    4,    4,   37,   37,   37,   37,   37,   37,   37,   37,
   37,   37,   37,   35,   35,   35,   35,   35,   42,   42,
   42,   42,   42,   42,   42,   40,   46,   46,    5,    5,
    5,    5,    5,   47,   47,   47,   36,   36,   48,   48,
   49,   49,   49,   49,   49,   49,   43,   43,   41,   41,
   41,   41,   41,   41,   41,   41,   41,   41,   41,   41,
   41,   41,   41,   41,   41,   41,   50,   50,   51,   51,
   15,   15,   15,   15,   44,   44,   39,   39,   52,   53,
   53,   53,   53,   53,   53,   53,   53,   53,   53,   54,
   54,   54,   54,   54,   54,   54,   54,   54,   54,   54,
   54,   54,   54,   54,   54,   55,   13,   13,   56,   56,
   56,   56,   56,   56,   56,   56,   56,   56,   56,   56,
   56,   59,   20,   20,   20,   20,   20,   20,   20,   20,
   20,   60,   27,   27,   61,   58,   58,   25,   62,   62,
   57,   57,   63,   64,   64,   38,   38,   65,   65,   66,
   66,   66,   66,   67,   67,   69,   69,   69,   69,   71,
   72,   72,   73,   73,   74,   74,   74,   74,   74,   74,
   75,   75,   75,   75,   75,   75,   75,   75,   75,   21,
   21,   76,   76,   76,   76,   76,   77,   77,   78,   79,
   79,   80,   81,   81,   82,   82,   45,   83,   84,   68,
   68,   85,   85,   85,   85,   85,   85,   85,   86,   86,
   86,   86,   87,   87,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,   70,   70,   70,   70,
   70,   70,   70,   70,   70,   70,
  };
   static readonly short [] yyLen = {           2,
    1,    1,    2,    3,    4,    4,    4,    4,    1,    1,
    1,    6,    5,    5,    6,    6,    7,    7,    6,    6,
    9,   10,   13,    9,   10,   10,   10,   10,    7,   11,
    9,   10,   11,    9,   10,    8,    5,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    3,    3,    3,    3,    6,    5,    1,    1,    3,
    1,    1,    1,    1,    1,    1,    1,    2,    3,    1,
    2,    3,    3,    3,    1,    1,    1,    1,    2,    1,
    1,    1,    1,    1,    1,    1,    3,    1,    1,    1,
    1,    4,    2,    3,    5,    1,    3,    1,    1,    1,
    1,    1,    1,    1,    1,    3,    4,    3,    5,    1,
    2,    5,    5,    4,    0,    4,    1,    3,    1,    1,
    7,    8,    1,    2,    4,    3,    5,    4,    1,    3,
    2,    4,    3,    2,    3,    3,    4,    4,    1,    1,
    1,    1,    2,    3,    2,    2,    1,    2,    4,    5,
    6,    6,    7,    1,    2,    1,    3,    2,    1,    3,
    1,    2,    2,    3,    1,    1,    1,    2,    1,    1,
    4,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    4,    1,    1,    4,    4,    2,    1,    3,    1,    1,
    2,    3,    2,    1,    1,    1,    1,    2,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    6,    9,   10,    8,    6,    6,    3,    3,    3,    5,
    6,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    2,    2,    1,    2,    1,    3,    2,    1,    2,
    1,    3,    1,    1,    3,    1,    2,    3,    1,    2,
    3,    1,    2,    1,    2,    1,    2,    3,    4,    1,
    3,    2,    1,    3,    2,    3,    3,    2,    4,    5,
    1,    1,    1,    1,    6,    9,   10,    6,    6,    1,
    3,    1,    1,    2,    2,    2,    1,    3,    5,    1,
    2,    3,    1,    2,    1,    1,    1,    1,    5,    1,
    3,    2,    7,    2,    2,    7,    1,    1,    8,    9,
    9,   10,    0,    1,    5,    6,   11,    5,    7,    5,
    5,    6,    4,    4,    5,    5,    6,    6,    7,    5,
    5,    6,    6,    7,    6,    7,    5,    6,    7,    7,
    8,    6,    4,    4,    6,    7,    6,    2,    6,    4,
    4,    4,    4,    6,    6,    7,    8,    7,    6,    6,
    6,    4,    3,    4,    7,    8,    8,    9,   10,    5,
    6,    5,    5,    6,    3,    4,    5,    6,    8,    4,
    5,    6,    6,    4,    5,    7,    8,    5,    6,   11,
    4,    5,    4,    5,    5,    4,
  };
   static readonly short [] yyDefRed = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    2,    9,   10,   11,    0,    0,    0,    0,    0,    0,
    0,    0,   99,  110,  102,  103,  104,  105,  101,    0,
  139,   42,   40,   43,   44,   45,   46,   47,   48,  317,
  177,  178,  179,    0,   41,    0,  169,  170,  175,  174,
  176,    0,  195,  196,    0,  172,  173,    0,    0,    0,
    0,  100,    0,    0,    0,    0,    0,  140,  141,    0,
    0,    0,    3,    0,    0,    0,  167,    0,    4,    0,
    0,  189,  190,   38,   39,   49,   50,    0,    0,    0,
    0,    0,    0,    0,  194,    0,    0,    0,    0,    0,
    0,  111,    0,    0,    0,  186,    0,   93,    0,    0,
    0,    0,    0,    0,    0,  145,    0,    0,    0,  182,
    0,    0,    0,   68,    0,    0,    0,    0,    0,    0,
    0,    0,  168,    5,    6,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  193,    0,    8,    0,    7,    0,
    0,    0,    0,    0,    0,    0,    0,  187,    0,   94,
    0,    0,    0,    0,  144,    0,  120,  106,    0,    0,
  117,    0,    0,   69,    0,  165,  166,  158,    0,    0,
  159,  199,    0,    0,    0,  197,    0,    0,    0,    0,
    0,    0,    0,    0,  245,  246,  244,  247,  248,  249,
  243,  226,  230,  251,  250,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  229,  227,  228,    0,    0,    0,
    0,  192,    0,    0,    0,    0,   51,    0,    0,    0,
   77,   76,   14,    0,    0,   70,   75,    0,  185,  181,
  184,  171,    0,    0,    0,    0,    0,    0,  107,    0,
    0,    0,   91,   90,   82,   80,   81,   83,   84,   85,
   86,   13,    0,   78,  162,    0,  157,    0,    0,    0,
    0,    0,    0,    0,  131,  198,    0,    0,    0,    0,
  150,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  256,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,   16,    0,    0,    0,
   71,   15,    0,  253,  116,  188,  112,   95,  113,  109,
  118,    0,    0,   12,   79,  164,  160,    0,    0,  126,
    0,    0,    0,    0,    0,    0,    0,  327,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  266,  269,    0,    0,
  274,    0,  320,  328,    0,  133,  146,    0,  151,    0,
    0,  152,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  239,    0,    0,    0,  237,  238,    0,
    0,    0,    0,    0,   64,   67,    0,   62,    0,   53,
   65,    0,   59,   61,   66,   63,   54,   55,   52,   18,
   17,   74,   73,   72,   87,  303,    0,  302,    0,  300,
  128,    0,    0,    0,  325,    0,    0,  322,    0,    0,
    0,    0,  324,  315,  316,    0,    0,  313,  334,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  318,  368,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  200,  201,  202,  203,  204,  205,  206,  207,  208,
  209,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  121,  267,    0,  275,    0,    0,    0,  132,  153,   36,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  257,    0,    0,    0,    0,    0,    0,    0,  258,
    0,  306,  304,  305,   92,    0,  127,  268,    0,  321,
  252,    0,    0,  280,    0,    0,    0,    0,    0,    0,
  314,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  210,  211,  222,  223,  224,  225,
  213,  215,  216,  217,  218,  214,  212,  220,  221,  219,
    0,    0,    0,  307,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  383,    0,    0,  122,   21,
    0,   31,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  240,    0,    0,    0,    0,    0,   57,    0,   60,
  301,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  363,    0,    0,  263,  264,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  384,    0,    0,    0,    0,    0,
    0,  236,  231,  235,  241,    0,   26,    0,    0,   56,
    0,    0,    0,  282,    0,    0,  283,    0,    0,    0,
    0,  335,    0,    0,  408,    0,    0,  393,    0,    0,
  412,    0,  397,    0,  414,  405,  401,    0,    0,  390,
    0,  341,  340,  392,  415,    0,    0,    0,    0,  338,
    0,    0,    0,    0,  242,  255,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  308,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  259,    0,  261,
    0,    0,    0,    0,  310,    0,    0,  285,    0,  281,
    0,    0,    0,    0,    0,  336,  365,  409,  374,  394,
  369,  398,  367,  402,  391,  342,  380,  403,  265,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  379,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  260,  234,    0,  323,    0,  326,  311,    0,  292,  293,
  294,    0,    0,    0,    0,  291,  287,  286,  284,    0,
    0,    0,    0,  339,    0,    0,    0,    0,  385,    0,
  406,    0,    0,    0,    0,    0,  366,  309,    0,  319,
    0,    0,    0,    0,    0,   23,    0,  232,  262,  312,
  289,    0,    0,    0,    0,    0,  329,    0,    0,    0,
  387,    0,    0,  386,  407,    0,    0,    0,  399,    0,
  233,  290,    0,    0,    0,    0,    0,  330,  331,    0,
    0,  388,    0,    0,    0,    0,    0,    0,    0,  332,
  389,    0,    0,    0,    0,    0,    0,    0,  337,  410,
    0,    0,  299,  295,  298,    0,    0,    0,    0,    0,
  296,  297,
  };
  protected static readonly short [] yyDgoto  = {             9,
   10,   11,   62,   12,   13,   14,  263,  234,  226,   63,
   90,  235,  584,   91,  279,   71,   93,  227,  430,  215,
  449,  432,  433,  434,  435,  236,  878,  264,  110,  111,
  170,  117,   95,  171,   15,  128,  184,  386,  280,  275,
   77,   67,   78,   68,   69,   16,  281,  180,  181,  159,
   96,  186,  532,  661,  216,  217,  879,  295,  846,  458,
  747,  880,  738,  739,  387,  388,  389,  390,  391,  392,
  585,  706,  806,  807,  957,  450,  663,  664,  884,  885,
  467,  468,  504,  668,  393,  394,  470,
  };
  protected static readonly short [] yySindex = {         1536,
   80,  -90,   93,  104,  139, 3426, -224, -267,    0, 1536,
    0,    0,    0,    0,   -8, 3964,  -70,  145,  174, 3290,
  -25,  -16,    0,    0,    0,    0,    0,    0,    0, -108,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  240,    0,  241,    0,    0,    0,    0,
    0,  245,    0,    0,   49,    0,    0,  246, 3729,  -45,
   53,    0, -137, -108,  289, 5703, 3714,    0,    0,   44,
   77,  298,    0,  324, 4014,   -4,    0, 4014,    0,  140,
  141,    0,    0,    0,    0,    0,    0,  326,  127, 5703,
  129,  -26,  -72,   90,    0, -108,  -29,  369,    1,  304,
  375,    0,  171, 5703, 5703,    0,  159,    0, -108,   -7,
  289,  181, 5703,  190,  156,    0,  433, 4519,  289,    0,
 5703,  289, 4014,    0,  214,  367, 4396, -159,    6, 4014,
  324,   25,    0,    0,    0,  239, 5703,  -26,  -26, 4221,
 5703,  -26, 5703,  -26,    0,  180,    0,  380,    0, -162,
  458,  381, 5519,  248,  469,  -30,  -27,    0,  182,    0,
 5703, 5703,   48, 5703,    0,  158,    0,    0, -108,  209,
    0,  289,  289,    0, 1736,    0,    0,    0, 5831,  210,
    0,    0,  195,  -83, -157,    0,  324,   28, -159,  324,
  480, 1988, 5703, 5703,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  -11,  503,  504,  507,  508,
 5718, 5726, 5718,  506,    0,    0,    0, 4221, 5703, 4221,
 5703,    0,  499,  500,  509,  216,    0, -162, 5549,    0,
    0,    0,    0,   35, 4221,    0,    0,  521,    0,    0,
    0,    0,  290, -108,  -32,  512,  -68,  311,    0, 5352,
  515,  539,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 1785,    0,    0, 2186,    0, 5234, -142, 5706,
  -82,  320, 5718,  327,    0,    0, -159,  324,  195,  195,
    0, -159,    0,  202,  542, -108, 4270,  -38, 5703, 5718,
 5718, 5718, 5718,    0,   60, 5622,  105,   84,  206,  544,
 4221,  545, 4221, 5391, 5433, 1744,    0, -162,  225,   39,
    0,    0, 5598,    0,    0,    0,    0,    0,    0,    0,
    0,  331, 5467,    0,    0,    0,    0,  332,  336,    0,
  536,  534, 5718, -176, 5718, 3757, 5718,    0, 4347,  175,
 4347,  175, 4347,  175, 5703, 2702,  175, 5703, 5703, 4347,
 3064, 3487, 5703, 5703, 5703, 5718, 5718, 5718, 5718, 5718,
 5703,  -37, 4533,  267, -246, 3878, 5718, 5718, 5718, 5718,
 5718, 5718, 5718, 5718, 5718, 5718, 5718, 5718,  562,  175,
 5703,  175, 3757,  173, 5703, 4371,    0,    0, 8640, -224,
    0, -224,    0,    0, 5706,    0,    0,  288,    0, -159,
  195,    0,  350, -187,  227,  565,  572, 5703,    3,  224,
  228,  232,  235,    0, 5718, 4221,  106,    0,    0,  382,
  257,  599,  266,  605,    0,    0,  610,    0,  383,    0,
    0,  528,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, -147,    0,  252,    0,
    0,  288, 8640, 9139,    0,  384, 4309,    0,  609,  798,
 4014, 4014,    0,    0,    0, 4221, 4347,    0,    0, 5703,
 4221, 4347, 5703, 4221, 4347, 5703, 4221, 5703, 4221, 5703,
 4221, 4221, 4221, 4347, 5703, 4221, 5703, 4221, 4221, 4221,
 4221,  612,  617,  623,  627,  629,    4, 5703, 5205,    5,
 5718,  631,    0,    0, 5703, 5703, 5703, 3593,    7,  264,
  286,  291,  294,  295,  299,  300,  301,  302,  305,  308,
  312,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0, 5703, 2443,  -76, 5703,  647, 5703, 4014, 1023, -225,
    0,    0, -224,    0,   77,   77, 4496,    0,    0,    0,
  422,  444,  446, -167, 5703,    8, 5718, 5703, 5703, 5703,
 5703,    0,  644, -224,  453,  330,  457,  335, 5302,    0,
 5433,    0,    0,    0,    0, 5467,    0,    0, -224,    0,
    0,  673,  452,    0,  679,  798,  798, 4014,  677, 4221,
    0, 4221,  681, 4221, 4221,  682, 4221, 4221,  683, 4221,
  684, 4221,  685,  688,  689, 4221, 4221,  690, 4221,  691,
  692,  696,  724, 5718, 5718, 5718, 1744, 5718, 1473,   11,
 5703,   13, 5703,  725, 5703, 4221, 4221,   19, 5703,   20,
 5718, 5703, 5703, 5703, 5703, 5703, 5703, 5703, 5703, 5703,
 5703, 5703, 5703, 4221,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
 5703, 4309,  729,    0, 4221,  346,  679,  679,  798,  798,
 5703, 5703,  647, 5703, 4014,    0, 5718,   77,    0,    0,
 -224,    0,  463,  520,   26, 5718,  735,  -23,  -22,  -21,
  -19,    0,   77, -224,  523, -224,  525,    0,  278,    0,
    0,   77,  452,  693, 4786,  393,  679,  679,  798, 4309,
  744,  747, 4309,  749,  750, 4309,  751,  754, 4309,  755,
 4309,  756, 4309, 4309, 4309,  759,  760, 4309,  761, 4309,
 4309, 4309, 4309,    0,  765,  766,    0,    0,  767,  768,
  556,  770, 5703,   27, 5703, 4221,  773, 5703,  774,  778,
  783, 5718,   29, 5718,  784, -108, -108, -108, -108, -108,
 -108, -108, -108, -108, -108, -108, -108,  785, 4221,  786,
  740,  790,  575,  195,  195,  679,  679,  798,  798,  679,
  679,  798,  798, 4014,    0,   77,  792, -224, 5718,  796,
 2340,    0,    0,    0,    0,   77,    0,   77, -224,    0,
  797, 5703, 5645,    0, 3521,  287,    0,  452,  456,  459,
  679,    0, 4309, 4309,    0, 4309, 4309,    0, 4309, 4309,
    0, 4309,    0, 4309,    0,    0,    0, 4309, 4309,    0,
 4309,    0,    0,    0,    0, 5718, 5718, 1744, 1744,    0,
  466,  804, 5703,  806,    0,    0,  472,  813,  482, 5703,
 5703,  817, 5718,  826, 2340, 4309,  828, 4309,    0, 5718,
  833,  195,  195,  195,  195,  679,  679,  195,  195,  679,
  679,  798,  484,   77,  835, 2340, 5718,    0,  297,    0,
   77,  452,  836, 5673,    0,  841, 2821,    0, 3664,    0,
 5688,  553,  452,  452,  497,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  767,
  626,  510,  -43,  513,  638,  522,  645, 4221, 4221, 2340,
  863, 2340,  867,    0, 4309,  820,  872,  657,  195,  195,
  195,  195,  195,  195,  195,  195,  679,  661, 2340,  309,
    0,    0, 2340,    0,  452,    0,    0, 5335,    0,    0,
    0,  540,  883,  885,  887,    0,    0,    0,    0,  452,
  595,  600,  452,    0,  671,  886,  550,  678,    0,  686,
    0,  606,  607,  867, 2340,  867,    0,    0, 5718,    0,
  195,  195,  195,  195,  195,    0,  421,    0,    0,    0,
    0,  427,  -35, 5718, 5718, 5718,    0,  452,  452,  616,
    0,  568,  695,    0,    0,  902,  912,  867,    0,  195,
    0,    0,  917, 5703,  569,  582,  583,    0,    0,  452,
  717,    0,  597,  603, 5703,   30, 5703, 5703, 5703,    0,
    0,  719,  736,   32, 5718,  -17,   -2,    2,    0,    0,
 5718,  942,    0,    0,    0,  951, 2340, 2340,  432,  445,
    0,    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0, 4064,    0,    0,  996,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  680,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 1127,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 1318,    0,    0,    0,    0,
    0, 1706,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 3807,  111,  727,    0,    0,    0,    0,    0,
 4142,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  680,    0,
  680,    0,  680,    0,    0,  664,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  281,    0,
    0,    0,    0,    0, 3886,    0,    0,    0,  728,    0,
    0,  732,    0,    0,    0,    0,    0,  680,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  200,
    0,    0,    0,    0,    0,  998,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  200,  200,    0,    0,    0,
    0,    0,    0,    0,    0, 1527,    0,    0,  263,    0,
    0,  743,  745,    0,    0,    0,    0,    0,  405,    0,
    0,    0,  -79,    0,  -46,    0,    0,    0,  255,    0,
    0,  564,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  200,    0,  200,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 1815,
    0,    0,    0,    0,  200,    0,    0,    0,    0,    0,
    0,    0,    0,  292,  200,    0,  200,    0,    0,    0,
 1802, 4182,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  474,    0,    0,  -42,    0,
    0,    0,    0,    0,    0,    0,  680,    0,  161,  440,
    0,  680,  958,    0,  517,  169,  200,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  200,    0,  200,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 5752,
    0, 5752,    0, 5752,    0,    0, 5752,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 2810,
    0, 5752,    0,    0,    0,    0,    0,    0,    0, 4629,
    0, 8745,    0,    0,    0,    0,    0,  -41,    0,  680,
  624,    0,    0,    0,    0,    0,    0,    0,  200,    0,
    0,    0,    0,    0,    0,  281,    0,    0,    0,    0,
    0,    0,    0,  741,    0,    0,   98,    0,  200,    0,
    0,  478,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  200,    0,    0,    0,
    0,  -36,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  200,    0,    0,    0,    0,
  200,    0,    0,  200,    0,    0,  200,    0,  200,    0,
  200,  200,  200,    0,    0,  200,    0,  200,  200,  200,
  200,    0,    0,    0,    0,    0,  200,    0,    0,  200,
    0,    0,    0,    0,    0,    0,    0,    0,  200,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  200,    0,    0,    0,    0,    0,  200,
    0,    0, 4754,    0, 4887, 8850,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  200,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 8955,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  200,
    0,  200,    0,  200,  200,    0,  200,  200,    0,  200,
    0,  200,    0,    0,    0,  200,  200,    0,  200,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  200,
    0,  200,    0,    0,    0,  200,  200,  200,    0,  200,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  200,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 5834,    0,  200,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 5012,    0,    0,
  821,    0,    0,    0,  200,    0,    0,  200,  200,  200,
  200,    0,  860,    0,    0,    0,    0,    0,    0,    0,
    0, 9060,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 5943,    0,
    0,    0,    0,  200,    0,  200,    0,    0,    0,    0,
    0,    0,  200,    0,    0, 1942, 2050, 2177, 2304, 2412,
 2539, 2666, 2774, 2901, 3028, 3136, 3263,    0,  200,    0,
    0,    0,    0, 6051,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  884,  988, 1082,    0,    0,
    0,    0,    0,    0,    0, 1376,    0, 1405, 1413,    0,
    0,    0,    0,    0,  200,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0, 6159, 6267, 6375, 6483,    0,    0, 6591,    0,    0,
    0,    0,    0, 1437,    0,    0,    0,    0,    0,    0,
 1450,    0,    0,    0,    0,  493,  200,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 6699,
    0,    0,    0,    0,    0,    0,    0,  200,  200,    0,
    0,    0, 6807,    0,    0,    0,    0,    0, 6915, 7023,
 7131,    0, 7239, 7347, 7455, 7563,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0, 7671,    0, 7779,    0,    0,    0,    0,
 7887, 7995, 8103, 8211, 8319,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 8427,    0, 8535,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  200,    0,    0,    0,    0,
    0,    0,    0,  200,    0,  200,  200,  200,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,
  };
  protected static readonly short [] yyGindex = {            0,
    0,  997,  923,    0,    0,    0,    0,  795,  800, 1005,
  809,   -6,  385,  108,  423, -125,    0,  718,  730, -297,
 -563,    0,  462,    0, -740,    0,  387,  771,  924,   22,
    0,   12,    0,  780,    0,   17,    0,  641, -116, -236,
   -3,    0,  -54,  975,  -59,    0, -197,    0,  772,    0,
    0, -158,    0,    0,    0,    0, -746,  -66,    0, -567,
 -559,   96,  203,  207, -351,  596,    0,  659,  662,  598,
 -491,  878,    0,  162,    0,  486,    0,  283,    0,  184,
 -133,   10,    0,  392,    0,  604,  231,
  };
  protected static readonly short [] yyTable = {            64,
  967,  408,   66,  116, 1014,  699,  431,  431,  438,   64,
  240,  185,  123,  241,  662,  704,  100,  792,  793,  794,
  130,  795,   60, 1043,  319,  448,  276,   65,  289,  317,
  148,   94,  330,  152,  542,  118,  161,   76, 1044,  270,
  395,  102, 1045,  123,  667,  118,  557,  619,  623,   70,
  631,  686,  109,   61,  743,  165,  745,   72,  271,   64,
   64,  883,  752,  754,  118,  749,  269,  118,   64,  789,
  843,   64,  853, 1035,  133, 1041,  129,  113,  313,  399,
  124,  130,  313,  140,  402,   59,  125,  119,  122,  456,
  676,  161,  551,   59,  707,  708,  129,  156,  157,  132,
   94,  505,   94,  415,   94,  506,  109,  146,  923,  457,
  276,  169,  683,  223,   64,  572,   64,  160,  224,  133,
  179,  276,  272,   64,  266,  573,  133,  415,  574,  940,
  192,  570,   82,   83,  218,  801,  220,  328,   63,   94,
   17,   63,  172,  883,  173,  297,  298,  189,  415,  415,
   98,  188,  115,   20,  244,  245,  273,  247,  101,  312,
  154,  548,  401,  441,   21,  182,  418,  182,   20,   40,
   98,  273,  246,  974,  121,  976,  419,  776,  777,   18,
   19,  780,  182,  842,  414,  844,  286,  287,  848,   79,
   70,  125,  987,  552,  123,  542,  138,   53,   54,   22,
   94,   98,  549,  277,  677,   80,  282,  472,   98,  475,
  115,  112,  301,  684,  303,  577,  484,  811,  225,   82,
   83,   63,  242,  274,   88,  243,   23,  129, 1008,  417,
  563,  124,  130,   98,   81,   24,  101,  125,  329,   98,
  892,  115,  276,  169,   25,   26,   27,   28,   29,  249,
  267,   30,  250,  268,  149,   97,  307,   98,  147,  308,
   99,  179,  133,   74,  545,  440,  546,  131,  308,   84,
   85,  448,  498,  431,  151,  101,  462,  187,  448,  103,
  104,  461,  409,  913,  105,  107,  866,  867,   94,  416,
  870,  871,  575,   94,  400,  576,  190,  429,  429,  278,
 1049, 1050,   98,  119,  115,  106,  119,  101,  311,  114,
  407,   88,  311, 1013,  944,  101,  447,  124,  800,  737,
   98,  576,  115,  538,   96,  961,  962,  890,  118,   64,
  891,   98,  466,  115,  471,   97,  474,  942,  477,  479,
  943,  481,  482,  483,  486,  488,  489,  490,  491,  988,
  125,  101,  943,  101,  497,  500,  101,  460,  126,  509,
  101,  101,  101,  127,  101,  136,  101,   98,   98,   98,
  288,   98,   98,   98,  534,   98,   64,  990,  540,  537,
  937,  101,   98,   98,  992,  101,  101,  101,  101,   98,
  101,  101,  997,  966,  101, 1000,  101,   98,  499,  134,
  135,  556,  101,  101,  536,   96,  145,  588,  150,  101,
  101,   94,  101,  101,  154,  101,   97,  678,   82,   83,
   82,   83,   84,   85,   84,   85,  153,  155,  154,  154,
 1018, 1019,  154,  154,  158,  154,   20,   20,  693,  156,
   20,   20,   92,   20,   98,  161,  115,  162,  161,   87,
  154,  154, 1030,  702,   64,   64,  164,  133,   20,   20,
  590, 1011,   40,  592,  943,  594,  595, 1012,  597,  598,
  576,  600, 1051,  602,  166,  943,  591,  606,  607,  675,
  609,  591,  586,  587,  591, 1052,  154,  174,  943,  175,
   98,  620,  622,  591,   20,  191,  222,  228,  626,  627,
  628,  630,  113,  229,  238,   98,   98,   98,   98,  239,
   88,  139,   88,  142,  163,  144,   19,  163,   58,  182,
  283,   58,  149,  149,  214,  644,  149,  149,  665,  149,
   64,   64,   64,  288,  671,  674,  288,   98,  248,  237,
  737,  737,  290,  291,  149,  149,  292,  293,  685,  299,
  183,  688,  689,  690,  691,  786,  304,  305,  669,  670,
  673,  315,  447,   37,  429,  316,  306,  320,  796,  447,
  798,  115,  473,  318,  476,  322,  285,  480,  323,  396,
  149,   64,  403,  398,  133,  404,  420,  421,  423,  956,
  445,  451,  452,  453,  454,  469,  539,  294,  294,  294,
  503,  273,  300,   98,  302,  115,  550,  553,  554,  709,
  533,  555,  535,  558,  744,  237,  746,  559,  746,  314,
  784,  560,  753,  155,  561,  756,  757,  758,  759,  760,
  761,  762,  763,  764,  765,  766,  767,  565,  564,  195,
  196,  197,  566,  198,  199,  200,  567,  201,  568,  569,
  448,  571,  583,  632,  769,  614,  581,  862,  863,  397,
  615,  204,  874,  115,   64,   64,  616,   64,   64,  205,
  617,  406,  618,  881,  625,  633,  410,  411,  412,  413,
  634,  680,  294,  635,  636,  422,  118,  424,  637,  638,
  639,  640,  778,  779,  641,  782,  783,  642,  805,  444,
  681,  643,  682,  276,  276,  692,  212,  156,  156,  694,
  695,  156,  156,  696,  156,  697,  703,  456,  705,  455,
  710,  459,  787,  463,  713,  716,  719,  721,  723,  156,
  156,  724,  725,  728,  730,  731,  746,  213,  746,  732,
   29,  746,  492,  493,  494,  495,  496,  929,  930,  502,
  889,  933,  934,  510,  511,  512,  513,  514,  515,  516,
  517,  518,  519,  520,  521,  156,  101,  733,  748,  211,
  276,  276,  771,  773,  276,  276,  788,   64,  791,  797,
  133,  799,  808,  802,   19,   19,  115,  813,   19,   19,
  814,   19,  816,  817,  819,  429,  887,  820,  822,  824,
  314,  562,  828,  829,  831,  872,   19,   19,  836,  837,
  838,  839,  840,  841,  981,  982,  847,  849,  983,  984,
   34,  850,  276,  276,  276,  276,  851,  855,  856,  858,
  662,   37,   37,  860,  861,  873,  746,  118,   37,  876,
  882,  582,   19,  918,  919,  893,  911,  912,  894,  914,
  589,  276,  915,   37,   37,  593,  916,  212,  596,   24,
  920,  599,  917,  601,  938,  603,  604,  605, 1010,  922,
  608,  925,  610,  611,  612,  613,  928,  429,  939,  945,
  948,  960,  964,   35,  805,  133,  963,  624,  213,   37,
  965,  155,  155,  968,  969,  155,  155,  137,  155,  141,
  143,  971,  970,  195,  196,  197,  975,  198,  199,  200,
  943,  201,  978,  155,  155,  979,  980,  986,  202,  203,
  211,  993,  994,  998,  995,  204,  996, 1001,  999, 1002,
 1003,  115,  115,  205, 1004,  115,  115,  115,  115, 1006,
 1007,  447, 1005,  687, 1020, 1023,  193,  194, 1021,  155,
  219, 1022,  221,  115,  115, 1024, 1025,  114, 1027,  115,
  115,  522,  523,  524,  525,  526,  527,  528,  529,  530,
  531, 1028, 1029, 1031,  711, 1039,  712, 1032,  714,  715,
  115,  717,  718, 1033,  720, 1047,  722,   22,  115,  115,
  726,  727, 1040,  729, 1048,    1,  115,  191,  134,  135,
  734,  735,  736,  136,  740,  742,   73, 1026,   29,   29,
  750,  751,   29,   29,  137,   29,  138,  755, 1034,  149,
 1036, 1037, 1038,  310,   89,  439,  206,  309,  768,  321,
   29,   29,  700,  325,  437,  547,  163,  115,  989,  327,
  909,  207,  208,  209,  210,  910,  770,  543,  578,  772,
  544,  579,  959,  859,  195,  196,  197,  580,  198,  199,
  200,  701,  201,  785,  781,    0,   29,  947,    0,  202,
  203,    0,  790,  666,    0,    0,  204,    0,    0,    0,
  114,   32,   60,    0,  205,    0,    0,    0,   34,   34,
    0,    0,   34,   34,  812,   34,    0,  815,    0,    0,
  818,    0,    0,  821,    0,  823,    0,  825,  826,  827,
   34,   34,  830,   61,  832,  833,  834,  835,    0,    0,
  191,    0,    0,    0,    0,    0,  115,   24,   24,    0,
  845,   24,   24,    0,   24,    0,    0,    0,  852,    0,
  854,    0,    0,    0,    0,   59,   34,    0,    0,   24,
   24,   35,   35,  857,    0,   35,   35,    0,   35,    0,
    0,    0,    0,    0,    0,    0,  115,  115,  115,    0,
  115,    0,    0,   35,   35,  875,    0,  206,    0,    0,
    0,    0,    0,    0,    0,   24,  115,    0,  115,  888,
    0,    0,  207,  208,  209,  210,    0,  896,  897,    0,
  898,  899,    0,  900,  901,    0,  902,    0,  903,   35,
    0,    0,  904,  905,    0,  906,    0,  115,    0,  115,
    0,    0,  907,  908,    0,  114,  114,    0,    0,  114,
  114,  114,  114,    0,    0,    0,    0,    0,    0,  921,
  924,    0,  926,    0,    0,    0,  927,  114,  114,  115,
    0,  115,    0,  114,  114,   22,   22,    0,    0,   22,
   22,    0,   22,  941,    0,  191,  191,    0,    0,  191,
  191,  191,  191,  958,  116,    0,    0,   22,   22,    0,
    0,    0,  114,  114,    0,    0,   23,  191,  191,    0,
    0,    0,    0,  191,  191,   24,    0,    0,    0,    0,
    0,    0,  972,  973,   25,   26,   27,   28,   29,  977,
    0,   30,    0,   22,    0,    0,  120,    0,    0,    0,
    0,    0,  191,  191,    0,    0,    0,    0,    0,   40,
   41,   42,    0,   43,   44,    0,    0,   46,    0,    0,
   47,   48,   49,   50,   51,    0,   52,    0,    0,   32,
   32,    0,    0,   32,   32,    0,   32,    0,  180,    0,
    0,  180,    0,    0,    0, 1009,    0,    0,    0,    0,
    0,   32,   32,    0,    0,   27,    0,  180,    0,    0,
 1015, 1016, 1017,  115,  115,  115,    0,  115,  115,  115,
    0,  115,    0,    0,  115,  115,    0,    0,  115,  115,
  115,  115,  115,   55,   25,  115,    0,   32,  180,    0,
    0,    0,   28,  115,    0,    0,  115,  115,    0,    0,
  115, 1042,    0,    0,    0,    0,    0, 1046,    0,    0,
    0,    0,    0,    0,  115,  115,   33,  115,  115,    0,
  180,  115,  115,  672,  115,  115,  115,  115,  115,   30,
  115,    0,  115,    0,    0,   56,   57,   58,    0,    0,
    0,    0,    0,  115,  115,  115,    0,  115,  115,    0,
    0,    0,  115,    0,  115,    0,    0,  115,  115,  115,
  115,  115,  115,  115,  115,  115,  115,    0,  115,  115,
    0,  115,  115,  115,  115,  115,  115,  115,  115,  115,
  115,  115,  115,  115,    0,  115,  115,  115,    0,    0,
    0,  115,  115,  115,  115,  115,    0,  115,  115,  115,
  115,  115,  115,  115,  115,  115,  108,    0,    0,    0,
    0,    0,   60,    0,    0,    0,  115,    0,    0,    0,
    0,    0,    0,    0,  774,  775,    0,    0,  115,  115,
  115,  115,    0,  115,    0,  115,  115,    0,    0,  115,
  115,  115,    0,   61,    0,    0,  108,  108,  108,    0,
  108,    0,    0,    0,  180,  180,  180,    0,  180,  180,
  180,  180,  180,    0,  809,  810,  108,    0,  108,  180,
  180,    0,    0,    0,    0,   59,  180,    0,    0,  180,
  180,  180,  180,  180,  180,    0,  180,    0,    0,    0,
    0,  180,    0,    0,    0,    0,    0,  108,    0,  108,
    0,    0,    0,    0,    0,  180,  180,    0,  180,  180,
    0,    0,  180,    0,    0,  180,  180,  180,  180,  180,
    0,  180,    0,   27,   27,    0,    0,   27,   27,  108,
   27,  108,    0,  864,  865,    0,    0,  868,  869,    0,
    0,    0,    0,    0,    0,   27,   27,    0,    0,    0,
    0,    0,   25,   25,    0,    0,   25,   25,    0,   25,
   28,   28,    0,    0,   28,   28,    0,   28,  895,    0,
    0,    0,    0,    0,   25,   25,    0,  180,  180,    0,
    0,   27,   28,   28,   33,   33,    0,    0,   33,   33,
    0,   33,  180,  180,  180,  180,    0,   30,   30,    0,
    0,   30,   30,    0,   30,    0,   33,   33,    0,    0,
   25,    0,    0,    0,    0,    0,  230,    0,   28,   30,
   30,    0,    0,  931,  932,   24,  183,  935,  936,  183,
  180,  180,  180,    0,   25,   26,   27,   28,   29,    0,
    0,   30,   33,    0,    0,  183,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   30,    0,    0,    0,    0,
    0,    0,    0,  108,  108,  108,    0,  108,  108,  108,
    0,  108,    0,    0,  108,  108,  183,    0,  108,  108,
  108,  108,  108,    1,    2,  108,    0,    3,    4,    0,
    5,    0,    0,  108,  985,    0,  108,  108,    0,    0,
  108,    0,    0,    0,    0,    6,    7,    0,  183,    0,
    0,    0,    0,    0,  108,  108,    0,  108,  108,    0,
    0,  108,  108,    0,  108,  108,  108,  108,  108,    0,
  108,    0,  108,  741,   99,  254,    0,    0,  254,    0,
  262,    8,    0,  108,  108,  108,    0,  108,  108,    0,
    0,    0,  108,    0,  108,    0,  254,  108,  108,  108,
  108,  108,  108,  108,  108,  108,  108,    0,  108,  108,
    0,  108,  108,  108,  108,  108,  108,  108,  108,  108,
  108,  108,  108,  108,    0,  108,  108,  254,    0,  324,
  108,  108,  108,  108,  108,  108,    0,  108,  108,  108,
  108,  108,  108,  108,  108,  108,   88,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  108,  254,    0,  254,
    0,    0,    0,    0,    0,    0,    0,    0,  108,  108,
  108,  108,    0,  108,    0,  108,  108,    0,    0,  108,
  108,  108,  183,  183,  183,    0,  183,  183,  183,  183,
  183,    0,    0,    0,    0,    0,    0,  183,  183,    0,
    0,   98,    0,  115,  183,    0,    0,  183,  183,  183,
  183,  183,  183,    0,  183,  251,    0,    0,    0,  183,
  195,  196,  197,    0,  198,  199,  200,    0,  201,    0,
    0,  252,    0,  183,  183,    0,  183,  183,    0,    0,
  183,    0,  204,  183,  183,  183,  183,  183,    0,  183,
  205,  284,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  251,    0,    0,  212,    0,    0,
    0,    0,    0,    0,    0,  253,  254,  255,    0,    0,
  252,   88,  256,  257,    0,  258,  259,  260,  261,    0,
    0,  254,    0,    0,    0,    0,    0,   88,  213,    0,
    0,    0,    0,    0,    0,  183,  183,  254,  254,   98,
    0,  115,    0,    0,    0,    0,    0,    0,    0,    0,
  183,  183,  183,  183,  253,  254,  255,    0,    0,    0,
  211,  256,  257,    0,  258,  259,  260,  261,    0,    0,
    0,   88,   88,   88,    0,    0,    0,    0,   88,   88,
    0,   88,   88,   88,   88,    0,    0,    0,  183,  183,
  183,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  254,  254,  254,    0,  254,  254,    0,    0,    0,
  254,    0,  254,    0,    0,  254,  254,  254,  254,  254,
  254,  254,  254,  254,  254,    0,  254,  254,    0,  254,
  254,  254,  254,  254,  254,  254,  254,  254,  254,  254,
  254,  254,    0,  254,  254,    0,    0,    0,    0,  254,
  254,  254,  254,  254,  254,  254,  254,  254,  254,  254,
  254,  254,    0,  254,  411,  411,   98,    0,  115,    0,
    0,    0,    0,    0,  254,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  254,  254,  254,  254,
    0,    0,    0,  254,  195,  196,  197,    0,  198,  199,
  200,    0,  201,    0,    0,    0,    0,    0,    0,  202,
  203,    0,    0,    0,    0,    0,  204,    0,    0,    0,
    0,    0,    0,    0,  205,    0,    0,    0,  411,  411,
  411,    0,  411,  411,    0,    0,    0,  411,    0,  411,
    0,    0,  411,  411,  411,  411,  411,  411,  411,  411,
  411,  411,    0,  411,  411,    0,  411,  411,  411,  411,
  411,  411,  411,  411,  411,  411,  411,  411,  411,    0,
  411,  411,  416,  416,    0,    0,  411,  411,  411,  411,
  411,    0,  411,  411,  411,  411,  411,  411,  411,    0,
  411,    0,    0,   98,    0,  115,    0,    0,    0,    0,
    0,  411,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  411,  411,  411,  411,  206,    0,    0,
  411,  101,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  207,  208,  209,  210,  416,  416,  416,    0,
  416,  416,    0,    0,    0,  416,    0,  416,    0,   60,
  416,  416,  416,  416,  416,  416,  416,  416,  416,  416,
    0,  416,  416,    0,  416,  416,  416,  416,  416,  416,
  416,  416,  416,  416,  416,  416,  416,    0,  416,  416,
   61,    0,    0,    0,  416,  416,  416,  416,  416,    0,
  416,  416,  416,  416,  416,  416,  416,    0,  416,  400,
  400,   98,    0,  115,    0,    0,    0,    0,  326,  416,
    0,    0,   59,    0,    0,    0,    0,    0,    0,    0,
    0,  416,  416,  416,  416,    0,    0,    0,  416,  120,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   41,   42,    0,   43,   44,    0,    0,
   46,    0,    0,   47,   48,   49,   50,   51,    0,   52,
    0,    0,    0,  400,  400,  400,    0,  400,  400,    0,
    0,    0,  400,    0,  400,    0,    0,  400,  400,  400,
  400,  400,  400,  400,  400,  400,  400,    0,  400,  400,
    0,  400,  400,  400,  400,  400,  400,  400,  400,  400,
  400,  400,  400,  400,    0,  400,  400,    0,    0,    0,
    0,  400,  400,  400,  400,  400,   55,  400,  400,  400,
  400,  400,  400,  400,    0,  400,  373,  373,   98,    0,
  115,    0,    0,    0,    0,    0,  400,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  400,  400,
  400,  400,    0,  230,    0,  400,    0,    0,    0,    0,
    0,    0,   24,    0,    0,    0,    0,    0,   56,   57,
   58,   25,   26,   27,   28,   29,    0,    0,   30,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  373,  373,  373,    0,  373,  373,    0,    0,    0,  373,
    0,  373,    0,    0,  373,  373,  373,  373,  373,  373,
  373,  373,  373,  373,    0,  373,  373,    0,  373,  373,
  373,  373,  373,  373,  373,  373,  373,  373,  373,  373,
  373,    0,  373,  373,  370,  370,    0,    0,  373,  373,
  373,  373,  373,    0,  373,  373,  373,  373,  373,  373,
  373,    0,  373,  645,  646,   98,    0,  115,    0,    0,
    0,    0,    0,  373,    0,    0,    0,    0,    0,    0,
    0,    0,  877,    0,    0,  373,  373,  373,  373,    0,
    0,    0,  373,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  370,  370,
  370,    0,  370,  370,    0,    0,    0,  370,    0,  370,
    0,   60,  370,  370,  370,  370,  370,  370,  370,  370,
  370,  370,    0,  370,  370,    0,  370,  370,  370,  370,
  370,  370,  370,  370,  370,  370,  370,  370,  370,    0,
  370,  370,   61,    0,    0,    0,  370,  370,  370,  370,
  370,    0,  370,  370,  370,  370,  370,  370,  370,    0,
  370,  371,  371,   98,    0,  115,    0,    0,    0,    0,
    0,  370,    0,    0,   59,    0,    0,    0,    0,    0,
    0,    0,    0,  370,  370,  370,  370,    0,    0,    0,
  370,    0,    0,    0,  647,  648,  649,  650,    0,    0,
    0,    0,    0,  651,  652,  653,  654,  655,  656,  657,
  658,  659,  660,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  371,  371,  371,    0,  371,
  371,    0,    0,    0,  371,    0,  371,    0,    0,  371,
  371,  371,  371,  371,  371,  371,  371,  371,  371,    0,
  371,  371,    0,  371,  371,  371,  371,  371,  371,  371,
  371,  371,  371,  371,  371,  371,    0,  371,  371,    0,
    0,    0,    0,  371,  371,  371,  371,  371,    0,  371,
  371,  371,  371,  371,  371,  371,    0,  371,  372,  372,
   98,    0,  115,    0,    0,    0,    0,    0,  371,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  371,  371,  371,  371,    0,   23,    0,  371,    0,    0,
    0,    0,    0,    0,   24,    0,    0,    0,    0,    0,
    0,    0,    0,   25,   26,   27,   28,   29,    0,    0,
   30,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  372,  372,  372,    0,  372,  372,    0,    0,
    0,  372,    0,  372,    0,    0,  372,  372,  372,  372,
  372,  372,  372,  372,  372,  372,    0,  372,  372,    0,
  372,  372,  372,  372,  372,  372,  372,  372,  372,  372,
  372,  372,  372,    0,  372,  372,  413,  413,    0,    0,
  372,  372,  372,  372,  372,    0,  372,  372,  372,  372,
  372,  372,  372,    0,  372,  478,    0,   98,    0,  115,
  333,  333,    0,    0,    0,  372,    0,  195,  196,  197,
    0,  198,  199,  200,    0,  201,    0,  372,  372,  372,
  372,    0,  949,  950,  372,    0,  951,    0,    0,  204,
    0,    0,    0,    0,    0,    0,    0,  205,    0,    0,
  413,  413,  413,    0,  413,  413,    0,    0,    0,  413,
    0,  413,    0,   60,  413,  413,  413,  413,  413,  413,
  413,  413,  413,  413,    0,  413,  413,    0,  413,  413,
  413,  413,  413,  413,  413,  413,  413,  413,  413,  413,
  413,    0,  413,  413,   61,    0,    0,    0,  413,  413,
  413,  413,  413,    0,  413,  413,  413,  413,  413,  413,
  413,    0,  413,  404,  404,   98,    0,  115,    0,    0,
    0,    0,    0,  413,    0,    0,   59,    0,    0,    0,
    0,    0,    0,    0,    0,  413,  413,  413,  413,    0,
  952,    0,  413,    0,  101,    0,    0,    0,    0,    0,
    0,  333,  333,  333,  333,  953,  954,  955,    0,    0,
  333,  333,  333,  333,  333,  333,  333,  333,  333,  333,
    0,    0,    0,    0,    0,    0,    0,  404,  404,  404,
    0,  404,  404,    0,    0,    0,  404,    0,  404,    0,
    0,  404,  404,  404,  404,  404,  404,  404,  404,  404,
  404,    0,  404,  404,    0,  404,  404,  404,  404,  404,
  404,  404,  404,  404,  404,  404,  404,  404,    0,  404,
  404,    0,    0,    0,    0,  404,  404,  404,  404,  404,
    0,  404,  404,  404,  404,  404,  404,  404,    0,  404,
  396,  396,   98,    0,  115,    0,    0,    0,    0,    0,
  404,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  404,  404,  404,  404,    0,   23,    0,  404,
    0,    0,    0,    0,    0,    0,   24,    0,    0,    0,
    0,    0,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  396,  396,  396,    0,  396,  396,
    0,    0,    0,  396,    0,  396,    0,    0,  396,  396,
  396,  396,  396,  396,  396,  396,  396,  396,    0,  396,
  396,    0,  396,  396,  396,  396,  396,  396,  396,  396,
  396,  396,  396,  396,  396,    0,  396,  396,  382,  382,
    0,    0,  396,  396,  396,  396,  396,    0,  396,  396,
  396,  396,  396,  396,  396,    0,  396,  485,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  396,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  396,
  396,  396,  396,    0,    0,    0,  396,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  382,  382,  382,    0,  382,  382,    0,    0,
    0,  382,    0,  382,    0,   60,  382,  382,  382,  382,
  382,  382,  382,  382,  382,  382,    0,  382,  382,    0,
  382,  382,  382,  382,  382,  382,  382,  382,  382,  382,
  382,  382,  382,    0,  382,  382,   61,    0,    0,    0,
  382,  382,  382,  382,  382,    0,  382,  382,  382,  382,
  382,  382,  382,    0,  382,  343,  343,    0,    0,    0,
    0,    0,    0,    0,    0,  382,   60,    0,   59,    0,
    0,    0,    0,    0,    0,    0,    0,  382,  382,  382,
  382,    0,    0,    0,  382,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   61,    0,    0,
  212,   82,   83,    0,    0,   84,   85,   86,   32,   33,
   34,   35,   36,   37,   38,   39,    0,    0,    0,  343,
  343,  343,   45,  343,  343,    0,    0,    0,  343,   59,
  343,  213,   87,  343,  343,  343,  343,  343,  343,  343,
  343,  343,  343,    0,  343,  343,    0,  343,  343,  343,
  343,  343,  343,  343,  343,  343,  343,  343,  343,  343,
    0,  343,  343,  211,    0,    0,    0,  343,  343,  343,
  343,  343,   60,  343,  343,  343,  343,  343,  343,  343,
    0,  343,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  343,   88,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   61,  343,  343,  343,  343,    0,   23,
    0,  343,    0,    0,    0,    0,    0,    0,   24,    0,
    0,    0,    0,    0,    0,    0,    0,   25,   26,   27,
   28,   29,    0,    0,   30,   59,    0,    0,    0,   31,
    0,    0,    0,  212,   32,   33,   34,   35,   36,   37,
   38,   39,   40,   41,   42,    0,   43,   44,   45,    0,
   46,    0,    0,   47,   48,   49,   50,   51,    0,   52,
   23,    0,    0,    0,  213,    0,    0,    0,    0,   24,
   53,   54,    0,    0,    0,    0,    0,    0,   25,   26,
   27,   28,   29,   60,    0,   30,    0,  195,  196,  197,
    0,  198,  199,  200,    0,  201,  211,    0,   60,    0,
    0,    0,  202,  203,    0,    0,    0,    0,    0,  204,
    0,    0,    0,    0,   61,    0,   55,  205,    0,    0,
    0,    0,    0,    0,  120,    0,   60,    0,    0,   61,
    0,    0,    0,    0,    0,    0,    0,    0,   41,   42,
    0,   43,   44,    0,    0,   46,   59,    0,   47,   48,
   49,   50,   51,    0,   52,    0,    0,   61,    0,    0,
  487,   59,    0,  108,    0,    0,   23,    0,   56,   57,
   58,    0,    0,    0,    0,   24,  142,    0,    0,    0,
    0,    0,    0,    0,   25,   26,   27,   28,   29,   59,
    0,   30,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  142,    0,    0,
  206,   55,    0,    0,  101,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  207,  208,  209,  210,    0,
  195,  196,  197,    0,  198,  199,  200,    0,  201,  142,
    0,    0,    0,    0,    0,  202,  203,   60,    0,    0,
    0,  629,  204,    0,    0,  143,    0,    0,    0,    0,
  205,    0,    0,   56,   57,   58,    0,  120,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   61,    0,
    0,   41,   42,    0,   43,   44,  143,   23,   46,    0,
    0,   47,   48,   49,   50,   51,   24,   52,    0,    0,
    0,    0,   23,    0,    0,   25,   26,   27,   28,   29,
   59,   24,   30,    0,    0,    0,    0,  120,  143,    0,
   25,   26,   27,   28,   29,    0,    0,   30,    0,    0,
   23,   41,   42,   60,   43,   44,    0,    0,   46,   24,
    0,   47,   48,   49,   50,   51,   87,   52,   25,   26,
   27,   28,   29,  206,   55,   30,    0,    0,    0,    0,
  120,    0,    0,    0,   61,    0,    0,    0,  207,  208,
  209,  210,    0,   40,   41,   42,    0,   43,   44,    0,
  142,   46,    0,   60,   47,   48,   49,   50,   51,  142,
   52,    0,    0,    0,    0,    0,   59,    0,  142,  142,
  142,  142,  142,    0,   55,  142,   56,   57,   58,    0,
  142,    0,    0,    0,   61,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  142,  142,    0,  142,  142,    0,
    0,  142,    0,  147,  142,  142,  142,  142,  142,  142,
  142,    0,    0,    0,    0,    0,   59,   55,    0,    0,
    0,   23,    0,    0,    0,    0,   56,   57,   58,  143,
   24,    0,    0,    0,  147,    0,    0,    0,  143,   25,
   26,   27,   28,   29,    0,    0,   30,  143,  143,  143,
  143,  143,    0,    0,  143,    0,    0,    0,    0,  143,
    0,    0,    0,    0,    0,    0,  147,  142,    0,   56,
   57,   58,    0,  143,  143,    0,  143,  143,    0,    0,
  143,  148,    0,  143,  143,  143,  143,  143,  143,  143,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  507,   23,    0,    0,
    0,    0,  148,    0,    0,    0,   24,    0,    0,  142,
  142,  142,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,    0,   75,    0,  508,
    0,    0,    0,    0,  148,    0,  143,    0,    0,    0,
    0,   41,   42,    0,   43,   44,    0,   23,   46,    0,
  212,   47,   48,   49,   50,   51,   24,   52,    0,    0,
    0,    0,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,   89,  120,    0,    0,
    0,  213,    0,  405,    0,    0,    0,    0,  143,  143,
  143,   41,   42,    0,   43,   44,    0,  147,   46,  212,
    0,   47,   48,   49,   50,   51,  147,   52,    0,    0,
    0,    0,    0,  211,   55,  147,  147,  147,  147,  147,
    0,    0,  147,    0,    0,    0,    0,  147,    0,    0,
  213,    0,    0,    0,    0,    0,    0,    0,  212,    0,
    0,  147,  147,    0,  147,  147,    0,    0,  147,    0,
    0,  147,  147,  147,  147,  147,    0,  147,    0,    0,
    0,    0,  211,    0,   55,    0,   56,   57,   58,  213,
    0,    0,    0,    0,    0,  148,   60,    0,    0,    0,
    0,    0,    0,    0,  148,    0,    0,    0,    0,    0,
    0,    0,    0,  148,  148,  148,  148,  148,    0,    0,
  148,  211,    0,    0,    0,  148,  178,   61,    0,    0,
    0,   89,    0,    0,  147,    0,   56,   57,   58,  148,
  148,    0,  148,  148,    0,   60,  148,   89,    0,  148,
  148,  148,  148,  148,    0,  148,    0,    0,    0,   59,
    0,    0,    0,    0,    0,    0,    0,  195,  196,  197,
    0,  198,  199,  200,    0,  201,   61,    0,    0,    0,
    0,    0,  202,  203,    0,  541,  147,  147,  147,  204,
    0,   89,   89,   89,    0,    0,    0,  205,   89,   89,
    0,   89,   89,   89,   89,    0,    0,    0,   59,    0,
    0,    0,  148,    0,    0,    0,  195,  196,  197,    0,
  198,  199,  200,    0,  201,    0,    0,    0,    0,    0,
    0,  202,  203,    0,    0,    0,    0,    0,  204,    0,
    0,    0,    0,    0,    0,    0,  205,    0,    0,  168,
    0,    0,    0,    0,    0,  195,  196,  197,    0,  198,
  199,  200,    0,  201,  148,  148,  148,    0,   60,    0,
  202,  203,    0,    0,    0,    0,    0,  204,    0,    0,
    0,    0,   60,    0,    0,  205,    0,    0,    0,    0,
  206,    0,    0,    0,  101,    0,    0,    0,    0,   61,
   23,    0,    0,    0,    0,  207,  208,  209,  210,   24,
  679,    0,    0,   61,    0,    0,    0,  331,   25,   26,
   27,   28,   29,    0,    0,   30,    0,    0,    0,    0,
    0,   59,    0,  332,    0,    0,    0,    0,    0,  206,
    0,    0,    0,  101,    0,   59,    0,    0,    0,   23,
    0,    0,    0,    0,  207,  208,  209,  210,   24,    0,
    0,    0,    0,  176,    0,    0,    0,   25,   26,   27,
   28,   29,    0,    0,   30,    0,    0,    0,  206,    0,
  177,    0,    0,    0,    0,  464,  465,    0,    0,    0,
    0,    0,    0,  207,  208,  209,  210,  333,  334,  335,
    0,  336,  337,    0,    0,    0,  338,    0,  339,    0,
    0,  340,  341,  342,  343,  344,  345,  346,  347,  348,
  349,    0,  350,  351,    0,  352,  353,  354,  355,  356,
  357,  358,  359,  360,  361,  362,  363,  364,    0,  365,
  366,    0,  331,  272,    0,  367,  368,  369,  370,  371,
    0,  372,  373,  374,  375,  376,  377,  378,  332,  379,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  380,    0,   23,    0,    0,    0,    0,    0,    0,    0,
    0,   24,  381,  382,  383,  384,  230,    0,    0,  385,
   25,   26,   27,   28,   29,   24,    0,   30,    0,    0,
    0,    0,    0,  167,   25,   26,   27,   28,   29,    0,
    0,   30,    0,    0,    0,    0,  804,    0,    0,    0,
    0,    0,  333,  334,  335,    0,  336,  337,    0,    0,
    0,  338,  501,  339,    0,   60,  340,  341,  342,  343,
  344,  345,  346,  347,  348,  349,    0,  350,  351,    0,
  352,  353,  354,  355,  356,  357,  358,  359,  360,  361,
  362,  363,  364,    0,  365,  366,   61,    0,  270,    0,
  367,  368,  369,  370,  371,  272,  372,  373,  374,  375,
  376,  377,  378,    0,  379,    0,    0,    0,    0,    0,
    0,  272,    0,    0,    0,  380,    0,    0,   59,    0,
    0,    0,    0,    0,    0,    0,    0,  381,  382,  383,
  384,    0,    0,    0,  385,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  272,  272,  272,    0,  272,
  272,    0,    0,    0,  272,    0,  272,    0,    0,  272,
  272,  272,  272,  272,  272,  272,  272,  272,  272,    0,
  272,  272,    0,  272,  272,  272,  272,  272,  272,  272,
  272,  272,  272,  272,  272,  272,    0,  272,  272,    0,
  270,  273,    0,  272,  272,  272,  272,  272,    0,  272,
  272,  272,  272,  272,  272,  272,  270,  272,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  272,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   23,
  272,  272,  272,  272,    0,    0,    0,  272,   24,    0,
    0,    0,    0,  803,    0,    0,    0,   25,   26,   27,
   28,   29,    0,    0,   30,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  270,  270,  270,    0,  270,  270,    0,    0,    0,  270,
    0,  270,    0,    0,  270,  270,  270,  270,  270,  270,
  270,  270,  270,  270,    0,  270,  270,    0,  270,  270,
  270,  270,  270,  270,  270,  270,  270,  270,  270,  270,
  270,    0,  270,  270,    0,    0,  271,    0,  270,  270,
  270,  270,  270,  273,  270,  270,  270,  270,  270,  270,
  270,    0,  270,    0,    0,    0,    0,    0,    0,  273,
    0,    0,    0,  270,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  270,  270,  270,  270,    0,
    0,    0,  270,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  273,  273,  273,    0,  273,  273,    0,
    0,    0,  273,    0,  273,    0,    0,  273,  273,  273,
  273,  273,  273,  273,  273,  273,  273,    0,  273,  273,
    0,  273,  273,  273,  273,  273,  273,  273,  273,  273,
  273,  273,  273,  273,   60,  273,  273,    0,  271,    0,
    0,  273,  273,  273,  273,  273,    0,  273,  273,  273,
  273,  273,  273,  273,  271,  273,    0,    0,    0,    0,
    0,    0,    0,   60,    0,   61,  273,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  273,  273,
  273,  273,    0,    0,    0,  273,    0,    0,    0,    0,
    0,    0,    0,    0,   61,    0,    0,   59,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  698,    0,    0,    0,    0,    0,  271,  271,
  271,    0,  271,  271,    0,    0,   59,  271,    0,  271,
    0,   60,  271,  271,  271,  271,  271,  271,  271,  271,
  271,  271,    0,  271,  271,  991,  271,  271,  271,  271,
  271,  271,  271,  271,  271,  271,  271,  271,  271,    0,
  271,  271,   61,    0,   60,    0,  271,  271,  271,  271,
  271,    0,  271,  271,  271,  271,  271,  271,  271,    0,
  271,   60,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  271,    0,    0,   59,   61,    0,    0,    0,    0,
    0,    0,    0,  271,  271,  271,  271,    0,    0,    0,
  271,    0,   61,    0,    0,    0,    0,    0,    0,    0,
   60,    0,    0,    0,    0,    0,    0,   59,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   23,    0,
    0,    0,    0,    0,   59,    0,    0,   24,    0,    0,
    0,   61,    0,    0,    0,    0,   25,   26,   27,   28,
   29,    0,   60,   30,    0,    0,    0,   23,    0,    0,
    0,    0,    0,    0,    0,    0,   24,    0,    0,    0,
    0,  176,    0,   59,  621,   25,   26,   27,   28,   29,
    0,    0,   30,   61,    0,    0,   60,    0,  177,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   59,    0,   61,  195,  196,
  197,    0,  198,  199,  200,   23,  201,    0,    0,    0,
    0,    0,    0,    0,   24,    0,    0,  446,   60,    0,
  204,    0,    0,   25,   26,   27,   28,   29,  205,   59,
   30,  195,  196,  197,    0,  198,  199,  200,   23,  201,
    0,    0,    0,    0,    0,    0,    0,   24,   60,   61,
  446,    0,    0,  204,    0,   23,   25,   26,   27,   28,
   29,  205,    0,   30,   24,    0,    0,    0,    0,    0,
    0,    0,    0,   25,   26,   27,   28,   29,    0,   61,
   30,   59,    0,  233,    0,    0,  167,  195,  196,  197,
  425,  198,  199,  200,   23,  426,    0,   60,    0,    0,
    0,    0,    0,   24,  427,    0,  428,    0,    0,  204,
    0,   59,   25,   26,   27,   28,   29,  205,    0,   30,
    0,   60,    0,    0,    0,    0,    0,    0,   61,  195,
  196,  197,  425,  198,  199,  200,   23,  426,    0,    0,
    0,    0,    0,    0,   60,   24,  436,    0,  428,    0,
    0,  204,   61,    0,   25,   26,   27,   28,   29,  205,
   59,   30,    0,  195,  196,  197,    0,  198,  199,  200,
   23,  201,   60,    0,    0,   61,    0,    0,    0,   24,
    0,    0,  446,    0,   59,  204,  108,   60,   25,   26,
   27,   28,   29,  205,    0,   30,    0,    0,    0,    0,
    0,    0,   60,   61,    0,  946,    0,   59,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   60,   61,    0,
    0,    0,  230,  231,    0,   60,    0,    0,    0,    0,
    0,   24,  232,   61,    0,   59,    0,    0,    0,    0,
   25,   26,   27,   28,   29,    0,    0,   30,   61,    0,
   59,  333,  230,  231,    0,    0,   61,    0,    0,    0,
    0,   24,  232,    0,    0,   59,    0,    0,    0,    0,
   25,   26,   27,   28,   29,    0,    0,   30,    0,    0,
   59,    0,  333,    0,    0,    0,    0,    0,  296,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  230,  442,    0,    0,    0,    0,    0,    0,    0,
   24,  443,    0,    0,  333,    0,    0,    0,    0,   25,
   26,   27,   28,   29,    0,  230,   30,    0,    0,    0,
    0,    0,    0,    0,   24,    0,    0,    0,    0,    0,
    0,    0,    0,   25,   26,   27,   28,   29,   23,    0,
   30,    0,    0,    0,    0,    0,    0,   24,  886,    0,
    0,    0,    0,    0,    0,    0,   25,   26,   27,   28,
   29,    0,    0,   30,    0,    0,   23,    0,    0,    0,
    0,    0,    0,    0,    0,   24,    0,    0,    0,    0,
    0,   23,    0,    0,   25,   26,   27,   28,   29,    0,
   24,   30,  331,    0,    0,  803,   23,    0,    0,   25,
   26,   27,   28,   29,    0,   24,   30,    0,  332,    0,
    0,  230,    0,    0,   25,   26,   27,   28,   29,  230,
   24,   30,    0,    0,    0,    0,    0,    0,   24,   25,
   26,   27,   28,   29,    0,    0,   30,   25,   26,   27,
   28,   29,    0,    0,   30,  333,    0,    0,    0,    0,
    0,    0,    0,    0,  333,    0,    0,    0,    0,    0,
    0,    0,    0,  333,  333,  333,  333,  333,    0,    0,
  333,    0,  333,  334,  335,    0,  336,  337,    0,    0,
    0,  338,    0,  339,    0,    0,  340,  341,  342,  343,
  344,  345,  346,  347,  348,  349,    0,  350,  351,    0,
  352,  353,  354,  355,  356,  357,  358,  359,  360,  361,
  362,  363,  364,    0,  365,  366,    0,    0,    0,    0,
  367,  368,  369,  370,  371,    0,  372,  373,  374,  375,
  376,  377,  378,  265,  379,    0,  395,  395,    0,    0,
    0,    0,    0,    0,    0,  380,    0,    0,    0,    0,
    0,    0,    0,    0,  120,    0,    0,  381,  382,  383,
  384,    0,    0,    0,  385,    0,    0,    0,   41,   42,
    0,   43,   44,    0,    0,   46,    0,    0,   47,   48,
   49,   50,   51,    0,   52,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  395,  395,  395,    0,  395,  395,    0,    0,    0,  395,
    0,  395,    0,    0,  395,  395,  395,  395,  395,  395,
  395,  395,  395,  395,    0,  395,  395,    0,  395,  395,
  395,  395,  395,  395,  395,  395,  395,  395,  395,  395,
  395,   55,  395,  395,  101,  364,  364,    0,  395,  395,
  395,  395,  395,    0,  395,  395,  395,  395,  395,  395,
  395,    0,  395,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  395,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  395,  395,  395,  395,    0,
    0,    0,  395,   56,   57,   58,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  364,
  364,  364,    0,  364,  364,    0,    0,    0,  364,    0,
  364,    0,    0,  364,  364,  364,  364,  364,  364,  364,
  364,  364,  364,    0,  364,  364,    0,  364,  364,  364,
  364,  364,  364,  364,  364,  364,  364,  364,  364,  364,
    0,  364,  364,  344,  344,    0,    0,  364,  364,  364,
  364,  364,    0,  364,  364,  364,  364,  364,  364,  364,
    0,  364,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  364,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  364,  364,  364,  364,    0,    0,
    0,  364,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  344,  344,  344,
    0,  344,  344,    0,    0,    0,  344,    0,  344,    0,
    0,  344,  344,  344,  344,  344,  344,  344,  344,  344,
  344,    0,  344,  344,    0,  344,  344,  344,  344,  344,
  344,  344,  344,  344,  344,  344,  344,  344,    0,  344,
  344,  350,  350,    0,    0,  344,  344,  344,  344,  344,
    0,  344,  344,  344,  344,  344,  344,  344,    0,  344,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  344,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  344,  344,  344,  344,    0,    0,    0,  344,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  350,  350,  350,    0,  350,
  350,    0,    0,    0,  350,    0,  350,    0,    0,  350,
  350,  350,  350,  350,  350,  350,  350,  350,  350,    0,
  350,  350,    0,  350,  350,  350,  350,  350,  350,  350,
  350,  350,  350,  350,  350,  350,    0,  350,  350,  345,
  345,    0,    0,  350,  350,  350,  350,  350,    0,  350,
  350,  350,  350,  350,  350,  350,    0,  350,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  350,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  350,  350,  350,  350,    0,    0,    0,  350,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  345,  345,  345,    0,  345,  345,    0,
    0,    0,  345,    0,  345,    0,    0,  345,  345,  345,
  345,  345,  345,  345,  345,  345,  345,    0,  345,  345,
    0,  345,  345,  345,  345,  345,  345,  345,  345,  345,
  345,  345,  345,  345,    0,  345,  345,  351,  351,    0,
    0,  345,  345,  345,  345,  345,    0,  345,  345,  345,
  345,  345,  345,  345,    0,  345,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  345,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  345,  345,
  345,  345,    0,    0,    0,  345,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  351,  351,  351,    0,  351,  351,    0,    0,    0,
  351,    0,  351,    0,    0,  351,  351,  351,  351,  351,
  351,  351,  351,  351,  351,    0,  351,  351,    0,  351,
  351,  351,  351,  351,  351,  351,  351,  351,  351,  351,
  351,  351,    0,  351,  351,  346,  346,    0,    0,  351,
  351,  351,  351,  351,    0,  351,  351,  351,  351,  351,
  351,  351,    0,  351,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  351,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  351,  351,  351,  351,
    0,    0,    0,  351,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  346,
  346,  346,    0,  346,  346,    0,    0,    0,  346,    0,
  346,    0,    0,  346,  346,  346,  346,  346,  346,  346,
  346,  346,  346,    0,  346,  346,    0,  346,  346,  346,
  346,  346,  346,  346,  346,  346,  346,  346,  346,  346,
    0,  346,  346,  357,  357,    0,    0,  346,  346,  346,
  346,  346,    0,  346,  346,  346,  346,  346,  346,  346,
    0,  346,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  346,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  346,  346,  346,  346,    0,    0,
    0,  346,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  357,  357,  357,
    0,  357,  357,    0,    0,    0,  357,    0,  357,    0,
    0,  357,  357,  357,  357,  357,  357,  357,  357,  357,
  357,    0,  357,  357,    0,  357,  357,  357,  357,  357,
  357,  357,  357,  357,  357,  357,  357,  357,    0,  357,
  357,  381,  381,    0,    0,  357,  357,  357,  357,  357,
    0,  357,  357,  357,  357,  357,  357,  357,    0,  357,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  357,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  357,  357,  357,  357,    0,    0,    0,  357,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  381,  381,  381,    0,  381,
  381,    0,    0,    0,  381,    0,  381,    0,    0,  381,
  381,  381,  381,  381,  381,  381,  381,  381,  381,    0,
  381,  381,    0,  381,  381,  381,  381,  381,  381,  381,
  381,  381,  381,  381,  381,  381,    0,  381,  381,  375,
  375,    0,    0,  381,  381,  381,  381,  381,    0,  381,
  381,  381,  381,  381,  381,  381,    0,  381,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  381,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  381,  381,  381,  381,    0,    0,    0,  381,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  375,  375,  375,    0,  375,  375,    0,
    0,    0,  375,    0,  375,    0,    0,  375,  375,  375,
  375,  375,  375,  375,  375,  375,  375,    0,  375,  375,
    0,  375,  375,  375,  375,  375,  375,  375,  375,  375,
  375,  375,  375,  375,    0,  375,  375,  352,  352,    0,
    0,  375,  375,  375,  375,  375,    0,  375,  375,  375,
  375,  375,  375,  375,    0,  375,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  375,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  375,  375,
  375,  375,    0,    0,    0,  375,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  352,  352,  352,    0,  352,  352,    0,    0,    0,
  352,    0,  352,    0,    0,  352,  352,  352,  352,  352,
  352,  352,  352,  352,  352,    0,  352,  352,    0,  352,
  352,  352,  352,  352,  352,  352,  352,  352,  352,  352,
  352,  352,    0,  352,  352,  347,  347,    0,    0,  352,
  352,  352,  352,  352,    0,  352,  352,  352,  352,  352,
  352,  352,    0,  352,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  352,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  352,  352,  352,  352,
    0,    0,    0,  352,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  347,
  347,  347,    0,  347,  347,    0,    0,    0,  347,    0,
  347,    0,    0,  347,  347,  347,  347,  347,  347,  347,
  347,  347,  347,    0,  347,  347,    0,  347,  347,  347,
  347,  347,  347,  347,  347,  347,  347,  347,  347,  347,
    0,  347,  347,  348,  348,    0,    0,  347,  347,  347,
  347,  347,    0,  347,  347,  347,  347,  347,  347,  347,
    0,  347,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  347,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  347,  347,  347,  347,    0,    0,
    0,  347,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  348,  348,  348,
    0,  348,  348,    0,    0,    0,  348,    0,  348,    0,
    0,  348,  348,  348,  348,  348,  348,  348,  348,  348,
  348,    0,  348,  348,    0,  348,  348,  348,  348,  348,
  348,  348,  348,  348,  348,  348,  348,  348,    0,  348,
  348,  353,  353,    0,    0,  348,  348,  348,  348,  348,
    0,  348,  348,  348,  348,  348,  348,  348,    0,  348,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  348,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  348,  348,  348,  348,    0,    0,    0,  348,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  353,  353,  353,    0,  353,
  353,    0,    0,    0,  353,    0,  353,    0,    0,  353,
  353,  353,  353,  353,  353,  353,  353,  353,  353,    0,
  353,  353,    0,  353,  353,  353,  353,  353,  353,  353,
  353,  353,  353,  353,  353,  353,    0,  353,  353,  362,
  362,    0,    0,  353,  353,  353,  353,  353,    0,  353,
  353,  353,  353,  353,  353,  353,    0,  353,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  353,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  353,  353,  353,  353,    0,    0,    0,  353,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  362,  362,  362,    0,  362,  362,    0,
    0,    0,  362,    0,  362,    0,    0,  362,  362,  362,
  362,  362,  362,  362,  362,  362,  362,    0,  362,  362,
    0,  362,  362,  362,  362,  362,  362,  362,  362,  362,
  362,  362,  362,  362,    0,  362,  362,  355,  355,    0,
    0,  362,  362,  362,  362,  362,    0,  362,  362,  362,
  362,  362,  362,  362,    0,  362,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  362,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  362,  362,
  362,  362,    0,    0,    0,  362,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  355,  355,  355,    0,  355,  355,    0,    0,    0,
  355,    0,  355,    0,    0,  355,  355,  355,  355,  355,
  355,  355,  355,  355,  355,    0,  355,  355,    0,  355,
  355,  355,  355,  355,  355,  355,  355,  355,  355,  355,
  355,  355,    0,  355,  355,  358,  358,    0,    0,  355,
  355,  355,  355,  355,    0,  355,  355,  355,  355,  355,
  355,  355,    0,  355,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  355,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  355,  355,  355,  355,
    0,    0,    0,  355,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  358,
  358,  358,    0,  358,  358,    0,    0,    0,  358,    0,
  358,    0,    0,  358,  358,  358,  358,  358,  358,  358,
  358,  358,  358,    0,  358,  358,    0,  358,  358,  358,
  358,  358,  358,  358,  358,  358,  358,  358,  358,  358,
    0,  358,  358,  378,  378,    0,    0,  358,  358,  358,
  358,  358,    0,  358,  358,  358,  358,  358,  358,  358,
    0,  358,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  358,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  358,  358,  358,  358,    0,    0,
    0,  358,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  378,  378,  378,
    0,  378,  378,    0,    0,    0,  378,    0,  378,    0,
    0,  378,  378,  378,  378,  378,  378,  378,  378,  378,
  378,    0,  378,  378,    0,  378,  378,  378,  378,  378,
  378,  378,  378,  378,  378,  378,  378,  378,    0,  378,
  378,  376,  376,    0,    0,  378,  378,  378,  378,  378,
    0,  378,  378,  378,  378,  378,  378,  378,    0,  378,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  378,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  378,  378,  378,  378,    0,    0,    0,  378,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  376,  376,  376,    0,  376,
  376,    0,    0,    0,  376,    0,  376,    0,    0,  376,
  376,  376,  376,  376,  376,  376,  376,  376,  376,    0,
  376,  376,    0,  376,  376,  376,  376,  376,  376,  376,
  376,  376,  376,  376,  376,  376,    0,  376,  376,  349,
  349,    0,    0,  376,  376,  376,  376,  376,    0,  376,
  376,  376,  376,  376,  376,  376,    0,  376,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  376,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  376,  376,  376,  376,    0,    0,    0,  376,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  349,  349,  349,    0,  349,  349,    0,
    0,    0,  349,    0,  349,    0,    0,  349,  349,  349,
  349,  349,  349,  349,  349,  349,  349,    0,  349,  349,
    0,  349,  349,  349,  349,  349,  349,  349,  349,  349,
  349,  349,  349,  349,    0,  349,  349,  354,  354,    0,
    0,  349,  349,  349,  349,  349,    0,  349,  349,  349,
  349,  349,  349,  349,    0,  349,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  349,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  349,  349,
  349,  349,    0,    0,    0,  349,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  354,  354,  354,    0,  354,  354,    0,    0,    0,
  354,    0,  354,    0,    0,  354,  354,  354,  354,  354,
  354,  354,  354,  354,  354,    0,  354,  354,    0,  354,
  354,  354,  354,  354,  354,  354,  354,  354,  354,  354,
  354,  354,    0,  354,  354,  356,  356,    0,    0,  354,
  354,  354,  354,  354,    0,  354,  354,  354,  354,  354,
  354,  354,    0,  354,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  354,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  354,  354,  354,  354,
    0,    0,    0,  354,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  356,
  356,  356,    0,  356,  356,    0,    0,    0,  356,    0,
  356,    0,    0,  356,  356,  356,  356,  356,  356,  356,
  356,  356,  356,    0,  356,  356,    0,  356,  356,  356,
  356,  356,  356,  356,  356,  356,  356,  356,  356,  356,
    0,  356,  356,  359,  359,    0,    0,  356,  356,  356,
  356,  356,    0,  356,  356,  356,  356,  356,  356,  356,
    0,  356,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  356,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  356,  356,  356,  356,    0,    0,
    0,  356,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  359,  359,  359,
    0,  359,  359,    0,    0,    0,  359,    0,  359,    0,
    0,  359,  359,  359,  359,  359,  359,  359,  359,  359,
  359,    0,  359,  359,    0,  359,  359,  359,  359,  359,
  359,  359,  359,  359,  359,  359,  359,  359,    0,  359,
  359,  360,  360,    0,    0,  359,  359,  359,  359,  359,
    0,  359,  359,  359,  359,  359,  359,  359,    0,  359,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  359,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  359,  359,  359,  359,    0,    0,    0,  359,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  360,  360,  360,    0,  360,
  360,    0,    0,    0,  360,    0,  360,    0,    0,  360,
  360,  360,  360,  360,  360,  360,  360,  360,  360,    0,
  360,  360,    0,  360,  360,  360,  360,  360,  360,  360,
  360,  360,  360,  360,  360,  360,    0,  360,  360,  377,
  377,    0,    0,  360,  360,  360,  360,  360,    0,  360,
  360,  360,  360,  360,  360,  360,    0,  360,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  360,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  360,  360,  360,  360,    0,    0,    0,  360,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  377,  377,  377,    0,  377,  377,    0,
    0,    0,  377,    0,  377,    0,    0,  377,  377,  377,
  377,  377,  377,  377,  377,  377,  377,    0,  377,  377,
    0,  377,  377,  377,  377,  377,  377,  377,  377,  377,
  377,  377,  377,  377,    0,  377,  377,  361,  361,    0,
    0,  377,  377,  377,  377,  377,    0,  377,  377,  377,
  377,  377,  377,  377,    0,  377,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  377,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  377,  377,
  377,  377,    0,    0,    0,  377,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  361,  361,  361,    0,  361,  361,    0,    0,    0,
  361,    0,  361,    0,    0,  361,  361,  361,  361,  361,
  361,  361,  361,  361,  361,    0,  361,  361,    0,  361,
  361,  361,  361,  361,  361,  361,  361,  361,  361,  361,
  361,  361,  332,  361,  361,    0,    0,    0,    0,  361,
  361,  361,  361,  361,    0,  361,  361,  361,  361,  361,
  361,  361,    0,  361,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  361,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  361,  361,  361,  361,
    0,    0,    0,  361,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  333,  334,  335,    0,
  336,  337,    0,    0,    0,  338,    0,  339,    0,    0,
  340,  341,  342,  343,  344,  345,  346,  347,  348,  349,
    0,  350,  351,    0,  352,  353,  354,  355,  356,  357,
  358,  359,  360,  361,  362,  363,  364,  276,  365,  366,
    0,    0,    0,    0,  367,  368,  369,  370,  371,    0,
  372,  373,  374,  375,  376,  377,  378,    0,  379,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  380,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  381,  382,  383,  384,    0,    0,    0,  385,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  276,  276,  276,    0,  276,  276,    0,    0,    0,
  276,    0,  276,    0,    0,  276,  276,  276,  276,  276,
  276,  276,  276,  276,  276,    0,  276,  276,    0,  276,
  276,  276,  276,  276,  276,  276,  276,  276,  276,  276,
  276,  276,  277,  276,  276,    0,    0,    0,    0,  276,
  276,  276,  276,  276,    0,  276,  276,  276,  276,  276,
  276,  276,    0,  276,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  276,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  276,  276,  276,  276,
    0,    0,    0,  276,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  277,  277,  277,    0,
  277,  277,    0,    0,    0,  277,    0,  277,    0,    0,
  277,  277,  277,  277,  277,  277,  277,  277,  277,  277,
    0,  277,  277,    0,  277,  277,  277,  277,  277,  277,
  277,  277,  277,  277,  277,  277,  277,  278,  277,  277,
    0,    0,    0,    0,  277,  277,  277,  277,  277,    0,
  277,  277,  277,  277,  277,  277,  277,    0,  277,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  277,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  277,  277,  277,  277,    0,    0,    0,  277,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  278,  278,  278,    0,  278,  278,    0,    0,    0,
  278,    0,  278,    0,    0,  278,  278,  278,  278,  278,
  278,  278,  278,  278,  278,    0,  278,  278,    0,  278,
  278,  278,  278,  278,  278,  278,  278,  278,  278,  278,
  278,  278,  279,  278,  278,    0,    0,    0,    0,  278,
  278,  278,  278,  278,    0,  278,  278,  278,  278,  278,
  278,  278,    0,  278,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  278,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  278,  278,  278,  278,
    0,    0,    0,  278,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  279,  279,  279,    0,
  279,  279,    0,    0,    0,  279,    0,  279,    0,    0,
  279,  279,  279,  279,  279,  279,  279,  279,  279,  279,
    0,  279,  279,    0,  279,  279,  279,  279,  279,  279,
  279,  279,  279,  279,  279,  279,  279,    0,  279,  279,
    0,    0,    0,    0,  279,  279,  279,  279,  279,    0,
  279,  279,  279,  279,  279,  279,  279,    0,  279,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  279,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  336,
    0,  279,  279,  279,  279,    0,  339,    0,  279,  340,
  341,  342,  343,  344,  345,  346,  347,  348,  349,    0,
  350,  351,    0,  352,  353,  354,  355,  356,  357,  358,
  359,  360,  361,  362,  363,  364,    0,  365,  366,    0,
    0,    0,    0,  367,  368,  369,  370,  371,    0,  372,
  373,  374,  375,  376,  377,  378,    0,  379,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  380,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  381,  382,  383,  384,    0,    0,    0,  385,
  };
  protected static readonly short [] yyCheck = {             6,
   44,   40,    6,   63,   40,  569,  304,  305,  306,   16,
   41,  128,   67,   41,   91,  583,   33,   41,   41,   41,
   75,   41,   60,   41,   93,  323,  185,    6,   40,   62,
   60,   20,  269,   33,  386,   40,   44,   16,   41,  123,
  123,   30,   41,  123,  536,   40,   44,   44,   44,  274,
   44,   44,   59,   91,   44,  115,   44,  325,  184,   66,
   67,  802,   44,   44,   40,  625,  183,   40,   75,   44,
   44,   78,   44,   44,   78,   44,  123,  123,   44,  277,
  123,  123,   44,   90,  282,  123,  123,   66,   67,  266,
  316,   44,  280,  123,  586,  587,   75,  104,  105,   78,
   89,  348,   91,   44,   93,  352,  113,   96,  855,  286,
  269,  118,  280,  276,  121,  263,  123,  125,  281,  123,
  127,  280,  280,  130,  179,  273,  130,   44,  276,  876,
  137,  429,  292,  293,  141,  703,  143,  280,   41,  128,
   61,   44,  121,  884,  123,  212,  213,  131,   44,   44,
   40,  130,   42,   61,  161,  162,  314,  164,  384,  125,
    0,  398,  279,  125,   61,  325,   62,  325,    0,  307,
   60,  314,  125,  920,   67,  922,   93,  669,  670,  270,
  271,  673,  325,  743,  125,  745,  193,  194,  748,  260,
  274,  274,  939,  381,  274,  547,   89,  335,  336,   61,
  189,   91,  400,  187,  430,   61,  190,  341,   40,  343,
   42,  257,  219,  381,  221,  452,  350,  709,  381,  292,
  293,  124,   41,  381,  384,   44,  264,  274,  975,  296,
  125,  274,  274,  123,   61,  273,  384,  274,  381,   40,
  808,   42,  401,  250,  282,  283,  284,  285,  286,   41,
   41,  289,   44,   44,    0,  281,   41,  274,  288,   44,
  277,  268,  266,  272,  390,   41,  392,  272,   44,  296,
  297,  569,  310,  571,  274,  384,  336,  272,  576,   40,
   40,  336,  289,  843,   40,   40,  778,  779,  277,  296,
  782,  783,   41,  282,  278,   44,  272,  304,  305,  272,
 1047, 1048,   40,   41,   42,  257,   44,  384,  274,  257,
  349,  384,  274,  349,  882,  384,  323,  274,   41,  617,
   40,   44,   42,  383,   44,  893,  894,   41,   40,  336,
   44,   40,  339,   42,  341,   44,  343,   41,  345,  346,
   44,  348,  349,  350,  351,  352,  353,  354,  355,   41,
  274,  384,   44,  384,  361,  362,  384,  336,   61,  366,
  384,  384,  384,   40,  384,   40,  384,  257,  258,  259,
  382,  261,  262,  263,  381,  265,  383,  945,  385,  383,
  872,  384,  272,  273,  948,  384,  384,  384,  384,  279,
  384,  384,  960,  437,  384,  963,  384,  287,  436,  260,
  260,  408,  384,  384,  383,  125,  317,  462,   40,  384,
  384,  400,  384,  384,   40,  384,  125,  543,  292,  293,
  292,  293,  296,  297,  296,  297,  123,  257,  268,  269,
  998,  999,  272,  273,  276,  275,  268,  269,  564,    0,
  272,  273,   20,  275,   40,   41,   42,  267,   44,  323,
  290,  291, 1020,  579,  461,  462,  267,  461,  290,  291,
  467,   41,  307,  470,   44,  472,  473,   41,  475,  476,
   44,  478,   41,  480,   42,   44,  467,  484,  485,  539,
  487,  472,  461,  462,  475,   41,  326,  274,   44,  123,
  380,  498,  499,  484,  326,  257,  317,   40,  505,  506,
  507,  508,  123,  123,  257,  395,  396,  397,  398,   41,
  384,   89,  384,   91,   41,   93,    0,   44,   41,  325,
   41,   44,  268,  269,  140,  532,  272,  273,  535,  275,
  537,  538,  539,   41,  538,  539,   44,  427,  381,  153,
  838,  839,   40,   40,  290,  291,   40,   40,  555,   44,
  128,  558,  559,  560,  561,  681,   58,   58,  537,  538,
  539,   41,  569,    0,  571,  276,   58,  257,  694,  576,
  696,  317,  342,   62,  344,   61,  192,  347,   40,  260,
  326,  588,  381,  257,  588,   44,  381,   44,   44,  887,
  260,  260,  257,   58,   61,  421,  424,  211,  212,  213,
  334,  314,  218,   40,  220,   42,  257,  381,   44,  588,
  380,   40,  382,  390,  621,  229,  623,  390,  625,  235,
  675,  390,  629,    0,  390,  632,  633,  634,  635,  636,
  637,  638,  639,  640,  641,  642,  643,  381,  257,  257,
  258,  259,   44,  261,  262,  263,  381,  265,   44,   40,
  948,  124,   44,  390,  661,   44,  273,  774,  775,  273,
   44,  279,  788,    0,  671,  672,   44,  674,  675,  287,
   44,  287,   44,  799,   44,  390,  290,  291,  292,  293,
  390,  260,  296,  390,  390,  301,   40,  303,  390,  390,
  390,  390,  671,  672,  390,  674,  675,  390,  705,  313,
  257,  390,  257,  862,  863,   62,   60,  268,  269,  257,
  381,  272,  273,  257,  275,  381,   44,  266,   40,  333,
   44,  335,  260,  337,   44,   44,   44,   44,   44,  290,
  291,   44,   44,   44,   44,   44,  743,   91,  745,   44,
    0,  748,  356,  357,  358,  359,  360,  864,  865,  363,
  805,  868,  869,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,  378,  326,  384,   44,   44,  123,
  929,  930,   44,  428,  933,  934,  257,  784,   44,  257,
  784,  257,  390,   91,  268,  269,  123,   44,  272,  273,
   44,  275,   44,   44,   44,  802,  803,   44,   44,   44,
  416,  415,   44,   44,   44,  784,  290,  291,   44,   44,
   44,   44,  257,   44,  931,  932,   44,   44,  935,  936,
    0,   44,  981,  982,  983,  984,   44,   44,   44,   44,
   91,  268,  269,   44,  260,   44,  843,   40,  275,   44,
   44,  457,  326,  850,  851,  390,  381,   44,  390,   44,
  466, 1010,  381,  290,  291,  471,   44,   60,  474,    0,
   44,  477,  381,  479,  381,  481,  482,  483,  985,   44,
  486,   44,  488,  489,  490,  491,   44,  884,   44,   44,
   40,  329,  257,    0,  891,  889,  390,  501,   91,  326,
  381,  268,  269,  381,  257,  272,  273,   89,  275,   91,
   92,  257,  381,  257,  258,  259,   44,  261,  262,  263,
   44,  265,   93,  290,  291,   44,  260,  257,  272,  273,
  123,  382,   40,  329,   40,  279,   40,  257,  329,   44,
  381,  268,  269,  287,  257,  272,  273,  274,  275,  334,
  334,  948,  257,  557,  329,   44,  138,  139,  381,  326,
  142,  257,  144,  290,  291,   44,   40,    0,  390,  296,
  297,  400,  401,  402,  403,  404,  405,  406,  407,  408,
  409,  390,  390,  257,  590,  257,  592,  381,  594,  595,
  317,  597,  598,  381,  600,   44,  602,    0,  325,  326,
  606,  607,  257,  609,   44,    0,  317,    0,  272,  272,
  614,  615,  616,  272,  618,  619,   10, 1014,  268,  269,
  626,  627,  272,  273,  272,  275,  272,  631, 1025,   97,
 1027, 1028, 1029,  229,   20,  308,  380,  228,  644,  250,
  290,  291,  571,  263,  305,  395,  113,   63,  943,  268,
  838,  395,  396,  397,  398,  839,  662,  389,  453,  665,
  389,  454,  891,  771,  257,  258,  259,  454,  261,  262,
  263,  576,  265,  677,  673,   -1,  326,  884,   -1,  272,
  273,   -1,  686,  427,   -1,   -1,  279,   -1,   -1,   -1,
  123,    0,   60,   -1,  287,   -1,   -1,   -1,  268,  269,
   -1,   -1,  272,  273,  710,  275,   -1,  713,   -1,   -1,
  716,   -1,   -1,  719,   -1,  721,   -1,  723,  724,  725,
  290,  291,  728,   91,  730,  731,  732,  733,   -1,   -1,
  123,   -1,   -1,   -1,   -1,   -1,    0,  268,  269,   -1,
  746,  272,  273,   -1,  275,   -1,   -1,   -1,  752,   -1,
  754,   -1,   -1,   -1,   -1,  123,  326,   -1,   -1,  290,
  291,  268,  269,  769,   -1,  272,  273,   -1,  275,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   40,   41,   42,   -1,
   44,   -1,   -1,  290,  291,  789,   -1,  380,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  326,   60,   -1,   62,  805,
   -1,   -1,  395,  396,  397,  398,   -1,  813,  814,   -1,
  816,  817,   -1,  819,  820,   -1,  822,   -1,  824,  326,
   -1,   -1,  828,  829,   -1,  831,   -1,   91,   -1,   93,
   -1,   -1,  836,  837,   -1,  268,  269,   -1,   -1,  272,
  273,  274,  275,   -1,   -1,   -1,   -1,   -1,   -1,  853,
  856,   -1,  858,   -1,   -1,   -1,  860,  290,  291,  123,
   -1,  125,   -1,  296,  297,  268,  269,   -1,   -1,  272,
  273,   -1,  275,  877,   -1,  268,  269,   -1,   -1,  272,
  273,  274,  275,  889,  317,   -1,   -1,  290,  291,   -1,
   -1,   -1,  325,  326,   -1,   -1,  264,  290,  291,   -1,
   -1,   -1,   -1,  296,  297,  273,   -1,   -1,   -1,   -1,
   -1,   -1,  918,  919,  282,  283,  284,  285,  286,  925,
   -1,  289,   -1,  326,   -1,   -1,  294,   -1,   -1,   -1,
   -1,   -1,  325,  326,   -1,   -1,   -1,   -1,   -1,  307,
  308,  309,   -1,  311,  312,   -1,   -1,  315,   -1,   -1,
  318,  319,  320,  321,  322,   -1,  324,   -1,   -1,  268,
  269,   -1,   -1,  272,  273,   -1,  275,   -1,   41,   -1,
   -1,   44,   -1,   -1,   -1,  979,   -1,   -1,   -1,   -1,
   -1,  290,  291,   -1,   -1,    0,   -1,   60,   -1,   -1,
  994,  995,  996,  257,  258,  259,   -1,  261,  262,  263,
   -1,  265,   -1,   -1,  268,  269,   -1,   -1,  272,  273,
  274,  275,  276,  381,    0,  279,   -1,  326,   91,   -1,
   -1,   -1,    0,  287,   -1,   -1,  290,  291,   -1,   -1,
  294, 1035,   -1,   -1,   -1,   -1,   -1, 1041,   -1,   -1,
   -1,   -1,   -1,   -1,  308,  309,    0,  311,  312,   -1,
  123,  315,  316,  421,  318,  319,  320,  321,  322,    0,
  324,   -1,  326,   -1,   -1,  433,  434,  435,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  381,   -1,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,  398,  399,    0,   -1,   -1,   -1,
   -1,   -1,   60,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  667,  668,   -1,   -1,  422,  423,
  424,  425,   -1,  427,   -1,  429,  430,   -1,   -1,  433,
  434,  435,   -1,   91,   -1,   -1,   40,   41,   42,   -1,
   44,   -1,   -1,   -1,  257,  258,  259,   -1,  261,  262,
  263,  264,  265,   -1,  707,  708,   60,   -1,   62,  272,
  273,   -1,   -1,   -1,   -1,  123,  279,   -1,   -1,  282,
  283,  284,  285,  286,  287,   -1,  289,   -1,   -1,   -1,
   -1,  294,   -1,   -1,   -1,   -1,   -1,   91,   -1,   93,
   -1,   -1,   -1,   -1,   -1,  308,  309,   -1,  311,  312,
   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,  322,
   -1,  324,   -1,  268,  269,   -1,   -1,  272,  273,  123,
  275,  125,   -1,  776,  777,   -1,   -1,  780,  781,   -1,
   -1,   -1,   -1,   -1,   -1,  290,  291,   -1,   -1,   -1,
   -1,   -1,  268,  269,   -1,   -1,  272,  273,   -1,  275,
  268,  269,   -1,   -1,  272,  273,   -1,  275,  811,   -1,
   -1,   -1,   -1,   -1,  290,  291,   -1,  380,  381,   -1,
   -1,  326,  290,  291,  268,  269,   -1,   -1,  272,  273,
   -1,  275,  395,  396,  397,  398,   -1,  268,  269,   -1,
   -1,  272,  273,   -1,  275,   -1,  290,  291,   -1,   -1,
  326,   -1,   -1,   -1,   -1,   -1,  264,   -1,  326,  290,
  291,   -1,   -1,  866,  867,  273,   41,  870,  871,   44,
  433,  434,  435,   -1,  282,  283,  284,  285,  286,   -1,
   -1,  289,  326,   -1,   -1,   60,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  326,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  257,  258,  259,   -1,  261,  262,  263,
   -1,  265,   -1,   -1,  268,  269,   91,   -1,  272,  273,
  274,  275,  276,  268,  269,  279,   -1,  272,  273,   -1,
  275,   -1,   -1,  287,  937,   -1,  290,  291,   -1,   -1,
  294,   -1,   -1,   -1,   -1,  290,  291,   -1,  123,   -1,
   -1,   -1,   -1,   -1,  308,  309,   -1,  311,  312,   -1,
   -1,  315,  316,   -1,  318,  319,  320,  321,  322,   -1,
  324,   -1,  326,  381,   40,   41,   -1,   -1,   44,   -1,
  125,  326,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   62,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   93,   -1,  125,
  384,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,  398,  399,  125,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,  123,   -1,  125,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,  427,   -1,  429,  430,   -1,   -1,  433,
  434,  435,  257,  258,  259,   -1,  261,  262,  263,  264,
  265,   -1,   -1,   -1,   -1,   -1,   -1,  272,  273,   -1,
   -1,   40,   -1,   42,  279,   -1,   -1,  282,  283,  284,
  285,  286,  287,   -1,  289,  260,   -1,   -1,   -1,  294,
  257,  258,  259,   -1,  261,  262,  263,   -1,  265,   -1,
   -1,  276,   -1,  308,  309,   -1,  311,  312,   -1,   -1,
  315,   -1,  279,  318,  319,  320,  321,  322,   -1,  324,
  287,   44,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  260,   -1,   -1,   60,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  320,  321,  322,   -1,   -1,
  276,  260,  327,  328,   -1,  330,  331,  332,  333,   -1,
   -1,  257,   -1,   -1,   -1,   -1,   -1,  276,   91,   -1,
   -1,   -1,   -1,   -1,   -1,  380,  381,  273,  274,   40,
   -1,   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  395,  396,  397,  398,  320,  321,  322,   -1,   -1,   -1,
  123,  327,  328,   -1,  330,  331,  332,  333,   -1,   -1,
   -1,  320,  321,  322,   -1,   -1,   -1,   -1,  327,  328,
   -1,  330,  331,  332,  333,   -1,   -1,   -1,  433,  434,
  435,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,   -1,   -1,   -1,   -1,  385,
  386,  387,  388,  389,  390,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,  273,  274,   40,   -1,   42,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,  257,  258,  259,   -1,  261,  262,
  263,   -1,  265,   -1,   -1,   -1,   -1,   -1,   -1,  272,
  273,   -1,   -1,   -1,   -1,   -1,  279,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  287,   -1,   -1,   -1,  337,  338,
  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,
   -1,   -1,  351,  352,  353,  354,  355,  356,  357,  358,
  359,  360,   -1,  362,  363,   -1,  365,  366,  367,  368,
  369,  370,  371,  372,  373,  374,  375,  376,  377,   -1,
  379,  380,  273,  274,   -1,   -1,  385,  386,  387,  388,
  389,   -1,  391,  392,  393,  394,  395,  396,  397,   -1,
  399,   -1,   -1,   40,   -1,   42,   -1,   -1,   -1,   -1,
   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  422,  423,  424,  425,  380,   -1,   -1,
  429,  384,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  395,  396,  397,  398,  337,  338,  339,   -1,
  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   60,
  351,  352,  353,  354,  355,  356,  357,  358,  359,  360,
   -1,  362,  363,   -1,  365,  366,  367,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,   -1,  379,  380,
   91,   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,
  391,  392,  393,  394,  395,  396,  397,   -1,  399,  273,
  274,   40,   -1,   42,   -1,   -1,   -1,   -1,  273,  410,
   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,  294,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  308,  309,   -1,  311,  312,   -1,   -1,
  315,   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   -1,   -1,   -1,
   -1,  385,  386,  387,  388,  389,  381,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,  273,  274,   40,   -1,
   42,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,  264,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,  433,  434,
  435,  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,  261,  262,   40,   -1,   42,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  383,   -1,   -1,  422,  423,  424,  425,   -1,
   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,
  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,
   -1,   60,  351,  352,  353,  354,  355,  356,  357,  358,
  359,  360,   -1,  362,  363,   -1,  365,  366,  367,  368,
  369,  370,  371,  372,  373,  374,  375,  376,  377,   -1,
  379,  380,   91,   -1,   -1,   -1,  385,  386,  387,  388,
  389,   -1,  391,  392,  393,  394,  395,  396,  397,   -1,
  399,  273,  274,   40,   -1,   42,   -1,   -1,   -1,   -1,
   -1,  410,   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,
  429,   -1,   -1,   -1,  402,  403,  404,  405,   -1,   -1,
   -1,   -1,   -1,  411,  412,  413,  414,  415,  416,  417,
  418,  419,  420,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,   -1,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,  273,  274,
   40,   -1,   42,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,  264,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,
   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,
  355,  356,  357,  358,  359,  360,   -1,  362,  363,   -1,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,
  385,  386,  387,  388,  389,   -1,  391,  392,  393,  394,
  395,  396,  397,   -1,  399,  364,   -1,   40,   -1,   42,
  261,  262,   -1,   -1,   -1,  410,   -1,  257,  258,  259,
   -1,  261,  262,  263,   -1,  265,   -1,  422,  423,  424,
  425,   -1,  272,  273,  429,   -1,  276,   -1,   -1,  279,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  287,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   60,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,   -1,  379,  380,   91,   -1,   -1,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,  273,  274,   40,   -1,   42,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,  123,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,
  380,   -1,  429,   -1,  384,   -1,   -1,   -1,   -1,   -1,
   -1,  402,  403,  404,  405,  395,  396,  397,   -1,   -1,
  411,  412,  413,  414,  415,  416,  417,  418,  419,  420,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,   -1,   -1,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
  273,  274,   40,   -1,   42,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,  264,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,
   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,
  353,  354,  355,  356,  357,  358,  359,  360,   -1,  362,
  363,   -1,  365,  366,  367,  368,  369,  370,  371,  372,
  373,  374,  375,  376,  377,   -1,  379,  380,  273,  274,
   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,  392,
  393,  394,  395,  396,  397,   -1,  399,  364,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,
  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,
   -1,  346,   -1,  348,   -1,   60,  351,  352,  353,  354,
  355,  356,  357,  358,  359,  360,   -1,  362,  363,   -1,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  380,   91,   -1,   -1,   -1,
  385,  386,  387,  388,  389,   -1,  391,  392,  393,  394,
  395,  396,  397,   -1,  399,  273,  274,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  410,   60,   -1,  123,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,
  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,   -1,   -1,
   60,  292,  293,   -1,   -1,  296,  297,  298,  299,  300,
  301,  302,  303,  304,  305,  306,   -1,   -1,   -1,  337,
  338,  339,  313,  341,  342,   -1,   -1,   -1,  346,  123,
  348,   91,  323,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  123,   -1,   -1,   -1,  385,  386,  387,
  388,  389,   60,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,  384,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   91,  422,  423,  424,  425,   -1,  264,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,  284,
  285,  286,   -1,   -1,  289,  123,   -1,   -1,   -1,  294,
   -1,   -1,   -1,   60,  299,  300,  301,  302,  303,  304,
  305,  306,  307,  308,  309,   -1,  311,  312,  313,   -1,
  315,   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,
  264,   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,  273,
  335,  336,   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,
  284,  285,  286,   60,   -1,  289,   -1,  257,  258,  259,
   -1,  261,  262,  263,   -1,  265,  123,   -1,   60,   -1,
   -1,   -1,  272,  273,   -1,   -1,   -1,   -1,   -1,  279,
   -1,   -1,   -1,   -1,   91,   -1,  381,  287,   -1,   -1,
   -1,   -1,   -1,   -1,  294,   -1,   60,   -1,   -1,   91,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  308,  309,
   -1,  311,  312,   -1,   -1,  315,  123,   -1,  318,  319,
  320,  321,  322,   -1,  324,   -1,   -1,   91,   -1,   -1,
  364,  123,   -1,  125,   -1,   -1,  264,   -1,  433,  434,
  435,   -1,   -1,   -1,   -1,  273,   60,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,  123,
   -1,  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,   -1,   -1,
  380,  381,   -1,   -1,  384,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  395,  396,  397,  398,   -1,
  257,  258,  259,   -1,  261,  262,  263,   -1,  265,  123,
   -1,   -1,   -1,   -1,   -1,  272,  273,   60,   -1,   -1,
   -1,  349,  279,   -1,   -1,   60,   -1,   -1,   -1,   -1,
  287,   -1,   -1,  433,  434,  435,   -1,  294,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,   -1,
   -1,  308,  309,   -1,  311,  312,   91,  264,  315,   -1,
   -1,  318,  319,  320,  321,  322,  273,  324,   -1,   -1,
   -1,   -1,  264,   -1,   -1,  282,  283,  284,  285,  286,
  123,  273,  289,   -1,   -1,   -1,   -1,  294,  123,   -1,
  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,   -1,
  264,  308,  309,   60,  311,  312,   -1,   -1,  315,  273,
   -1,  318,  319,  320,  321,  322,  323,  324,  282,  283,
  284,  285,  286,  380,  381,  289,   -1,   -1,   -1,   -1,
  294,   -1,   -1,   -1,   91,   -1,   -1,   -1,  395,  396,
  397,  398,   -1,  307,  308,  309,   -1,  311,  312,   -1,
  264,  315,   -1,   60,  318,  319,  320,  321,  322,  273,
  324,   -1,   -1,   -1,   -1,   -1,  123,   -1,  282,  283,
  284,  285,  286,   -1,  381,  289,  433,  434,  435,   -1,
  294,   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  308,  309,   -1,  311,  312,   -1,
   -1,  315,   -1,   60,  318,  319,  320,  321,  322,  323,
  324,   -1,   -1,   -1,   -1,   -1,  123,  381,   -1,   -1,
   -1,  264,   -1,   -1,   -1,   -1,  433,  434,  435,  264,
  273,   -1,   -1,   -1,   91,   -1,   -1,   -1,  273,  282,
  283,  284,  285,  286,   -1,   -1,  289,  282,  283,  284,
  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,  294,
   -1,   -1,   -1,   -1,   -1,   -1,  123,  381,   -1,  433,
  434,  435,   -1,  308,  309,   -1,  311,  312,   -1,   -1,
  315,   60,   -1,  318,  319,  320,  321,  322,  323,  324,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  349,  264,   -1,   -1,
   -1,   -1,   91,   -1,   -1,   -1,  273,   -1,   -1,  433,
  434,  435,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,  294,   -1,  382,
   -1,   -1,   -1,   -1,  123,   -1,  381,   -1,   -1,   -1,
   -1,  308,  309,   -1,  311,  312,   -1,  264,  315,   -1,
   60,  318,  319,  320,  321,  322,  273,  324,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,  125,  294,   -1,   -1,
   -1,   91,   -1,   44,   -1,   -1,   -1,   -1,  433,  434,
  435,  308,  309,   -1,  311,  312,   -1,  264,  315,   60,
   -1,  318,  319,  320,  321,  322,  273,  324,   -1,   -1,
   -1,   -1,   -1,  123,  381,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,  294,   -1,   -1,
   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   60,   -1,
   -1,  308,  309,   -1,  311,  312,   -1,   -1,  315,   -1,
   -1,  318,  319,  320,  321,  322,   -1,  324,   -1,   -1,
   -1,   -1,  123,   -1,  381,   -1,  433,  434,  435,   91,
   -1,   -1,   -1,   -1,   -1,  264,   60,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,  123,   -1,   -1,   -1,  294,   41,   91,   -1,   -1,
   -1,  260,   -1,   -1,  381,   -1,  433,  434,  435,  308,
  309,   -1,  311,  312,   -1,   60,  315,  276,   -1,  318,
  319,  320,  321,  322,   -1,  324,   -1,   -1,   -1,  123,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  257,  258,  259,
   -1,  261,  262,  263,   -1,  265,   91,   -1,   -1,   -1,
   -1,   -1,  272,  273,   -1,  125,  433,  434,  435,  279,
   -1,  320,  321,  322,   -1,   -1,   -1,  287,  327,  328,
   -1,  330,  331,  332,  333,   -1,   -1,   -1,  123,   -1,
   -1,   -1,  381,   -1,   -1,   -1,  257,  258,  259,   -1,
  261,  262,  263,   -1,  265,   -1,   -1,   -1,   -1,   -1,
   -1,  272,  273,   -1,   -1,   -1,   -1,   -1,  279,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  287,   -1,   -1,   41,
   -1,   -1,   -1,   -1,   -1,  257,  258,  259,   -1,  261,
  262,  263,   -1,  265,  433,  434,  435,   -1,   60,   -1,
  272,  273,   -1,   -1,   -1,   -1,   -1,  279,   -1,   -1,
   -1,   -1,   60,   -1,   -1,  287,   -1,   -1,   -1,   -1,
  380,   -1,   -1,   -1,  384,   -1,   -1,   -1,   -1,   91,
  264,   -1,   -1,   -1,   -1,  395,  396,  397,  398,  273,
  125,   -1,   -1,   91,   -1,   -1,   -1,  257,  282,  283,
  284,  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,
   -1,  123,   -1,  273,   -1,   -1,   -1,   -1,   -1,  380,
   -1,   -1,   -1,  384,   -1,  123,   -1,   -1,   -1,  264,
   -1,   -1,   -1,   -1,  395,  396,  397,  398,  273,   -1,
   -1,   -1,   -1,  278,   -1,   -1,   -1,  282,  283,  284,
  285,  286,   -1,   -1,  289,   -1,   -1,   -1,  380,   -1,
  295,   -1,   -1,   -1,   -1,  349,  350,   -1,   -1,   -1,
   -1,   -1,   -1,  395,  396,  397,  398,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,   -1,  257,  125,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,  273,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  273,  422,  423,  424,  425,  264,   -1,   -1,  429,
  282,  283,  284,  285,  286,  273,   -1,  289,   -1,   -1,
   -1,   -1,   -1,  295,  282,  283,  284,  285,  286,   -1,
   -1,  289,   -1,   -1,   -1,   -1,   41,   -1,   -1,   -1,
   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,
   -1,  346,  310,  348,   -1,   60,  351,  352,  353,  354,
  355,  356,  357,  358,  359,  360,   -1,  362,  363,   -1,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  380,   91,   -1,  125,   -1,
  385,  386,  387,  388,  389,  257,  391,  392,  393,  394,
  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,
   -1,  273,   -1,   -1,   -1,  410,   -1,   -1,  123,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,
  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,   -1,
  257,  125,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,  273,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  264,
  422,  423,  424,  425,   -1,   -1,   -1,  429,  273,   -1,
   -1,   -1,   -1,  278,   -1,   -1,   -1,  282,  283,  284,
  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,   -1,  379,  380,   -1,   -1,  125,   -1,  385,  386,
  387,  388,  389,  257,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,  273,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,
   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   60,  379,  380,   -1,  257,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,  273,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   60,   -1,   91,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   91,   -1,   -1,  123,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   41,   -1,   -1,   -1,   -1,   -1,  337,  338,
  339,   -1,  341,  342,   -1,   -1,  123,  346,   -1,  348,
   -1,   60,  351,  352,  353,  354,  355,  356,  357,  358,
  359,  360,   -1,  362,  363,   41,  365,  366,  367,  368,
  369,  370,  371,  372,  373,  374,  375,  376,  377,   -1,
  379,  380,   91,   -1,   60,   -1,  385,  386,  387,  388,
  389,   -1,  391,  392,  393,  394,  395,  396,  397,   -1,
  399,   60,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  410,   -1,   -1,  123,   91,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,
  429,   -1,   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   60,   -1,   -1,   -1,   -1,   -1,   -1,  123,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  264,   -1,
   -1,   -1,   -1,   -1,  123,   -1,   -1,  273,   -1,   -1,
   -1,   91,   -1,   -1,   -1,   -1,  282,  283,  284,  285,
  286,   -1,   60,  289,   -1,   -1,   -1,  264,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,
   -1,  278,   -1,  123,  310,  282,  283,  284,  285,  286,
   -1,   -1,  289,   91,   -1,   -1,   60,   -1,  295,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  123,   -1,   91,  257,  258,
  259,   -1,  261,  262,  263,  264,  265,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  273,   -1,   -1,  276,   60,   -1,
  279,   -1,   -1,  282,  283,  284,  285,  286,  287,  123,
  289,  257,  258,  259,   -1,  261,  262,  263,  264,  265,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,   60,   91,
  276,   -1,   -1,  279,   -1,  264,  282,  283,  284,  285,
  286,  287,   -1,  289,  273,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   91,
  289,  123,   -1,  125,   -1,   -1,  295,  257,  258,  259,
  260,  261,  262,  263,  264,  265,   -1,   60,   -1,   -1,
   -1,   -1,   -1,  273,  274,   -1,  276,   -1,   -1,  279,
   -1,  123,  282,  283,  284,  285,  286,  287,   -1,  289,
   -1,   60,   -1,   -1,   -1,   -1,   -1,   -1,   91,  257,
  258,  259,  260,  261,  262,  263,  264,  265,   -1,   -1,
   -1,   -1,   -1,   -1,   60,  273,  274,   -1,  276,   -1,
   -1,  279,   91,   -1,  282,  283,  284,  285,  286,  287,
  123,  289,   -1,  257,  258,  259,   -1,  261,  262,  263,
  264,  265,   60,   -1,   -1,   91,   -1,   -1,   -1,  273,
   -1,   -1,  276,   -1,  123,  279,  125,   60,  282,  283,
  284,  285,  286,  287,   -1,  289,   -1,   -1,   -1,   -1,
   -1,   -1,   60,   91,   -1,   93,   -1,  123,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   60,   91,   -1,
   -1,   -1,  264,  265,   -1,   60,   -1,   -1,   -1,   -1,
   -1,  273,  274,   91,   -1,  123,   -1,   -1,   -1,   -1,
  282,  283,  284,  285,  286,   -1,   -1,  289,   91,   -1,
  123,   60,  264,  265,   -1,   -1,   91,   -1,   -1,   -1,
   -1,  273,  274,   -1,   -1,  123,   -1,   -1,   -1,   -1,
  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,   -1,
  123,   -1,   91,   -1,   -1,   -1,   -1,   -1,  123,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  264,  265,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  273,  274,   -1,   -1,  123,   -1,   -1,   -1,   -1,  282,
  283,  284,  285,  286,   -1,  264,  289,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,  264,   -1,
  289,   -1,   -1,   -1,   -1,   -1,   -1,  273,  274,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,
  286,   -1,   -1,  289,   -1,   -1,  264,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,
   -1,  264,   -1,   -1,  282,  283,  284,  285,  286,   -1,
  273,  289,  257,   -1,   -1,  278,  264,   -1,   -1,  282,
  283,  284,  285,  286,   -1,  273,  289,   -1,  273,   -1,
   -1,  264,   -1,   -1,  282,  283,  284,  285,  286,  264,
  273,  289,   -1,   -1,   -1,   -1,   -1,   -1,  273,  282,
  283,  284,  285,  286,   -1,   -1,  289,  282,  283,  284,
  285,  286,   -1,   -1,  289,  264,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,
   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,
  355,  356,  357,  358,  359,  360,   -1,  362,  363,   -1,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  380,   -1,   -1,   -1,   -1,
  385,  386,  387,  388,  389,   -1,  391,  392,  393,  394,
  395,  396,  397,  273,  399,   -1,  273,  274,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  294,   -1,   -1,  422,  423,  424,
  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,  308,  309,
   -1,  311,  312,   -1,   -1,  315,   -1,   -1,  318,  319,
  320,  321,  322,   -1,  324,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,  381,  379,  380,  384,  273,  274,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,
   -1,   -1,  429,  433,  434,  435,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,  273,  379,  380,   -1,   -1,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,
  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,
  351,  352,  353,  354,  355,  356,  357,  358,  359,  360,
   -1,  362,  363,   -1,  365,  366,  367,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,  273,  379,  380,
   -1,   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,
  391,  392,  393,  394,  395,  396,  397,   -1,  399,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,  273,  379,  380,   -1,   -1,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,
  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,
  351,  352,  353,  354,  355,  356,  357,  358,  359,  360,
   -1,  362,  363,   -1,  365,  366,  367,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,  273,  379,  380,
   -1,   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,
  391,  392,  393,  394,  395,  396,  397,   -1,  399,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,  273,  379,  380,   -1,   -1,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,
  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,
  351,  352,  353,  354,  355,  356,  357,  358,  359,  360,
   -1,  362,  363,   -1,  365,  366,  367,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,   -1,  379,  380,
   -1,   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,
  391,  392,  393,  394,  395,  396,  397,   -1,  399,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  341,
   -1,  422,  423,  424,  425,   -1,  348,   -1,  429,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,   -1,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,
  };

#line 1466 "Iril/IR/IR.jay"

}

#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 System.Diagnostics.Debug.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
 class Token {
  public const int INTEGER = 257;
  public const int HEX_INTEGER = 258;
  public const int FLOAT_LITERAL = 259;
  public const int STRING = 260;
  public const int TRUE = 261;
  public const int FALSE = 262;
  public const int UNDEF = 263;
  public const int VOID = 264;
  public const int NULL = 265;
  public const int LABEL = 266;
  public const int X = 267;
  public const int SOURCE_FILENAME = 268;
  public const int TARGET = 269;
  public const int DATALAYOUT = 270;
  public const int TRIPLE = 271;
  public const int GLOBAL_SYMBOL = 272;
  public const int LOCAL_SYMBOL = 273;
  public const int META_SYMBOL = 274;
  public const int META_SYMBOL_DEF = 275;
  public const int SYMBOL = 276;
  public const int DISTINCT = 277;
  public const int METADATA = 278;
  public const int CONSTANT_BYTES = 279;
  public const int SECTION = 280;
  public const int TYPE = 281;
  public const int HALF = 282;
  public const int FLOAT = 283;
  public const int DOUBLE = 284;
  public const int X86_FP80 = 285;
  public const int INTEGER_TYPE = 286;
  public const int ZEROINITIALIZER = 287;
  public const int OPAQUE = 288;
  public const int PTR = 289;
  public const int DEFINE = 290;
  public const int DECLARE = 291;
  public const int UNNAMED_ADDR = 292;
  public const int LOCAL_UNNAMED_ADDR = 293;
  public const int NOALIAS = 294;
  public const int ELLIPSIS = 295;
  public const int GLOBAL = 296;
  public const int CONSTANT = 297;
  public const int PRIVATE = 298;
  public const int INTERNAL = 299;
  public const int EXTERNAL = 300;
  public const int LINKONCE = 301;
  public const int LINKONCE_ODR = 302;
  public const int WEAK = 303;
  public const int WEAK_ODR = 304;
  public const int APPENDING = 305;
  public const int COMMON = 306;
  public const int FASTCC = 307;
  public const int SIGNEXT = 308;
  public const int ZEROEXT = 309;
  public const int VOLATILE = 310;
  public const int RETURNED = 311;
  public const int DEREFERENCEABLE = 312;
  public const int AVAILABLE_EXTERNALLY = 313;
  public const int PERSONALITY = 314;
  public const int SRET = 315;
  public const int CLEANUP = 316;
  public const int EXTERNALLY_INITIALIZED = 317;
  public const int NONNULL = 318;
  public const int NOCAPTURE = 319;
  public const int WRITEONLY = 320;
  public const int READONLY = 321;
  public const int READNONE = 322;
  public const int HIDDEN = 323;
  public const int BYVAL = 324;
  public const int ATTRIBUTE_GROUP_REF = 325;
  public const int ATTRIBUTES = 326;
  public const int NORECURSE = 327;
  public const int NOUNWIND = 328;
  public const int UNWIND = 329;
  public const int SPECULATABLE = 330;
  public const int SSP = 331;
  public const int UWTABLE = 332;
  public const int ARGMEMONLY = 333;
  public const int SEQ_CST = 334;
  public const int DSO_LOCAL = 335;
  public const int DSO_PREEMPTABLE = 336;
  public const int RET = 337;
  public const int BR = 338;
  public const int SWITCH = 339;
  public const int INDIRECTBR = 340;
  public const int INVOKE = 341;
  public const int RESUME = 342;
  public const int CATCHSWITCH = 343;
  public const int CATCHRET = 344;
  public const int CLEANUPRET = 345;
  public const int UNREACHABLE = 346;
  public const int FNEG = 347;
  public const int ADD = 348;
  public const int NUW = 349;
  public const int NSW = 350;
  public const int FADD = 351;
  public const int SUB = 352;
  public const int FSUB = 353;
  public const int MUL = 354;
  public const int FMUL = 355;
  public const int UDIV = 356;
  public const int SDIV = 357;
  public const int FDIV = 358;
  public const int UREM = 359;
  public const int SREM = 360;
  public const int FREM = 361;
  public const int SHL = 362;
  public const int LSHR = 363;
  public const int EXACT = 364;
  public const int ASHR = 365;
  public const int AND = 366;
  public const int OR = 367;
  public const int XOR = 368;
  public const int EXTRACTELEMENT = 369;
  public const int INSERTELEMENT = 370;
  public const int SHUFFLEVECTOR = 371;
  public const int EXTRACTVALUE = 372;
  public const int INSERTVALUE = 373;
  public const int ALLOCA = 374;
  public const int LOAD = 375;
  public const int STORE = 376;
  public const int FENCE = 377;
  public const int CMPXCHG = 378;
  public const int ATOMICRMW = 379;
  public const int GETELEMENTPTR = 380;
  public const int ALIGN = 381;
  public const int INBOUNDS = 382;
  public const int INRANGE = 383;
  public const int ADDRSPACE = 384;
  public const int TRUNC = 385;
  public const int ZEXT = 386;
  public const int SEXT = 387;
  public const int FPTRUNC = 388;
  public const int FPEXT = 389;
  public const int TO = 390;
  public const int FPTOUI = 391;
  public const int FPTOSI = 392;
  public const int UITOFP = 393;
  public const int SITOFP = 394;
  public const int PTRTOINT = 395;
  public const int INTTOPTR = 396;
  public const int BITCAST = 397;
  public const int ADDRSPACECAST = 398;
  public const int ICMP = 399;
  public const int EQ = 400;
  public const int NE = 401;
  public const int UGT = 402;
  public const int UGE = 403;
  public const int ULT = 404;
  public const int ULE = 405;
  public const int SGT = 406;
  public const int SGE = 407;
  public const int SLT = 408;
  public const int SLE = 409;
  public const int FCMP = 410;
  public const int OEQ = 411;
  public const int OGT = 412;
  public const int OGE = 413;
  public const int OLT = 414;
  public const int OLE = 415;
  public const int ONE = 416;
  public const int ORD = 417;
  public const int UEQ = 418;
  public const int UNE = 419;
  public const int UNO = 420;
  public const int FAST = 421;
  public const int PHI = 422;
  public const int SELECT = 423;
  public const int CALL = 424;
  public const int TAIL = 425;
  public const int VA_ARG = 426;
  public const int ASM = 427;
  public const int SIDEEFFECT = 428;
  public const int LANDINGPAD = 429;
  public const int CATCH = 430;
  public const int CATCHPAD = 431;
  public const int CLEANUPPAD = 432;
  public const int NOUNDEF = 433;
  public const int IMMARG = 434;
  public const int CAPTURES = 435;
  public const int ATOMIC = 436;
  public const int MONOTONIC = 437;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }
  internal class yyUnexpectedEof : yyException {
    public yyUnexpectedEof (string message) : base (message) {
    }
    public yyUnexpectedEof () : base ("") {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
