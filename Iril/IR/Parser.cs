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
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage function_addr global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage function_addr global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage visibility_style global_kind type",
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
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value ',' SECTION STRING",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value ',' SECTION STRING ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type",
//t    "global_kind : GLOBAL",
//t    "global_kind : CONSTANT",
//t    "external_linkage : EXTERNAL",
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
//t    "parameter_attribute : SWIFTSELF",
//t    "parameter_attribute : SWIFTERROR",
//t    "parameter_attribute : SWIFTASYNC",
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
//t    "lblock : SYMBOL ':' block",
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
//t    "calling_convention : SWIFTCC",
//t    "calling_convention : SWIFTTAILCC",
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
    "SWIFTCC","SWIFTTAILCC","SWIFTSELF","SWIFTERROR","SWIFTASYNC",
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
#line 65 "Iril/IR/IR.jay"
  {
        module.SourceFilename = (string)yyVals[0+yyTop];
    }
  break;
case 5:
#line 69 "Iril/IR/IR.jay"
  {
        module.TargetDatalayout = (string)yyVals[0+yyTop];
    }
  break;
case 6:
#line 73 "Iril/IR/IR.jay"
  {
        module.TargetTriple = (string)yyVals[0+yyTop];
    }
  break;
case 7:
#line 77 "Iril/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = (StructureType)yyVals[0+yyTop];
    }
  break;
case 8:
#line 81 "Iril/IR/IR.jay"
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
#line 102 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-4+yyTop]] = new List<object> (0);
    }
  break;
case 15:
#line 106 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 16:
  case_16();
  break;
case 17:
#line 115 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 18:
  case_18();
  break;
case 19:
#line 127 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 20:
#line 132 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-4+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 21:
#line 136 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-7+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 22:
#line 140 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 23:
#line 144 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 24:
#line 148 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 25:
#line 152 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-3+yyTop], isConstant: (bool)yyVals[-3+yyTop]);
    }
  break;
case 26:
#line 156 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 27:
#line 160 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 28:
#line 164 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-12+yyTop], (LType)yyVals[-7+yyTop], (Value)yyVals[-6+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-8+yyTop]);
    }
  break;
case 29:
#line 168 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 30:
#line 172 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 31:
#line 176 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 32:
#line 180 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 33:
#line 184 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-7+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 34:
#line 188 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-6+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: (bool)yyVals[-4+yyTop], isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 35:
#line 192 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: (bool)yyVals[-8+yyTop], isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 36:
#line 197 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: (bool)yyVals[-3+yyTop], isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 37:
#line 201 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-6+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 38:
#line 205 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-6+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 39:
#line 209 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-11+yyTop], (LType)yyVals[-7+yyTop], (Value)yyVals[-6+yyTop], isPrivate: (bool)yyVals[-9+yyTop], isExternal: false, isConstant: (bool)yyVals[-8+yyTop]);
    }
  break;
case 40:
#line 213 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 41:
#line 217 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 42:
#line 221 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-8+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 43:
#line 225 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 44:
#line 229 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 45:
#line 233 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-7+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-5+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 46:
#line 237 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-4+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-2+yyTop], isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 47:
#line 241 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 48:
#line 242 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 49:
#line 250 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 50:
#line 254 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 51:
#line 255 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 52:
#line 256 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 53:
#line 257 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 54:
#line 258 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 55:
#line 259 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 56:
#line 260 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 57:
#line 261 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 58:
#line 265 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 59:
#line 269 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 60:
  case_60();
  break;
case 61:
  case_61();
  break;
case 62:
#line 286 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 63:
#line 287 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 64:
#line 288 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 65:
#line 292 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-5+yyTop], yyVals[-3+yyTop]);
    }
  break;
case 66:
#line 296 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-4+yyTop], yyVals[-2+yyTop]);
    }
  break;
case 77:
#line 325 "Iril/IR/IR.jay"
  {
        yyVal = NewSyms (yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 78:
#line 329 "Iril/IR/IR.jay"
  {
        yyVal = SymsAdd (yyVals[-2+yyTop], yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 79:
#line 336 "Iril/IR/IR.jay"
  {
        yyVal = NewList (yyVals[0+yyTop]);
    }
  break;
case 80:
#line 340 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 81:
#line 344 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 82:
#line 348 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 83:
#line 352 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 102:
#line 386 "Iril/IR/IR.jay"
  {
        yyVal = LiteralStructureType.Empty;
    }
  break;
case 103:
#line 390 "Iril/IR/IR.jay"
  {
        yyVal = new LiteralStructureType (false, (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 104:
#line 394 "Iril/IR/IR.jay"
  {
        yyVal = new PackedStructureType ((List<LType>)yyVals[-2+yyTop]);
    }
  break;
case 105:
#line 401 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 106:
#line 405 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 108:
#line 410 "Iril/IR/IR.jay"
  { yyVal = VoidType.Void; }
  break;
case 111:
#line 416 "Iril/IR/IR.jay"
  { yyVal = FloatType.Half; }
  break;
case 112:
#line 417 "Iril/IR/IR.jay"
  { yyVal = FloatType.Float; }
  break;
case 113:
#line 418 "Iril/IR/IR.jay"
  { yyVal = FloatType.Double; }
  break;
case 114:
#line 419 "Iril/IR/IR.jay"
  { yyVal = FloatType.X86_FP80; }
  break;
case 115:
#line 423 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-2+yyTop], Enumerable.Empty<LType>());
    }
  break;
case 116:
#line 427 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-3+yyTop], (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 117:
#line 431 "Iril/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-2+yyTop], 0);
    }
  break;
case 118:
#line 435 "Iril/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-4+yyTop], 0);
    }
  break;
case 119:
#line 439 "Iril/IR/IR.jay"
  {
        yyVal = new NamedType ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 120:
#line 443 "Iril/IR/IR.jay"
  {
        yyVal = PointerType.OpaquePointer;
    }
  break;
case 121:
#line 447 "Iril/IR/IR.jay"
  {
        yyVal = new VectorType ((int)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 122:
#line 451 "Iril/IR/IR.jay"
  {
        yyVal = new ArrayType ((long)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 126:
#line 467 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 127:
#line 471 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 129:
#line 479 "Iril/IR/IR.jay"
  {
        yyVal = VarArgsType.VarArgs;
    }
  break;
case 130:
  case_130();
  break;
case 131:
  case_131();
  break;
case 143:
#line 513 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create ((object)true, yyVals[0+yyTop]);
    }
  break;
case 144:
#line 517 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create ((object)true, yyVals[0+yyTop]);
    }
  break;
case 145:
#line 521 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 146:
#line 525 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 147:
#line 529 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 148:
#line 536 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 149:
#line 540 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 150:
#line 544 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 155:
#line 555 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 158:
#line 567 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-2+yyTop], (GlobalSymbol)yyVals[-1+yyTop], (IEnumerable<Parameter>)yyVals[0+yyTop]);
    }
  break;
case 159:
#line 571 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 160:
#line 575 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 161:
#line 579 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 162:
#line 583 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 166:
#line 593 "Iril/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 167:
#line 594 "Iril/IR/IR.jay"
  { yyVal = Enumerable.Empty<Parameter> (); }
  break;
case 168:
#line 601 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Parameter)yyVals[0+yyTop]);
    }
  break;
case 169:
#line 605 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Parameter)yyVals[0+yyTop]);
    }
  break;
case 170:
#line 612 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[0+yyTop]);
    }
  break;
case 171:
#line 616 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter ((LocalSymbol)yyVals[0+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 172:
#line 620 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[-1+yyTop]);
    }
  break;
case 173:
#line 624 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter ((LocalSymbol)yyVals[0+yyTop], (LType)yyVals[-2+yyTop]);
    }
  break;
case 174:
#line 628 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, IntegerType.I32);
    }
  break;
case 175:
#line 632 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, VarArgsType.VarArgs);
    }
  break;
case 177:
#line 640 "Iril/IR/IR.jay"
  {
        yyVal = ((ParameterAttributes)yyVals[-1+yyTop]) | ((ParameterAttributes)yyVals[0+yyTop]);
    }
  break;
case 178:
#line 644 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NonNull; }
  break;
case 179:
#line 645 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 180:
#line 646 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 181:
#line 647 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.None; }
  break;
case 182:
#line 648 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.None; }
  break;
case 183:
#line 649 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.None; }
  break;
case 184:
#line 650 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoUndef; }
  break;
case 185:
#line 651 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ImmediateArgument; }
  break;
case 186:
#line 652 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadOnly; }
  break;
case 187:
#line 653 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.WriteOnly; }
  break;
case 188:
#line 654 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadNone; }
  break;
case 189:
#line 655 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.SignExtend; }
  break;
case 190:
#line 656 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ZeroExtend; }
  break;
case 191:
#line 657 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Returned; }
  break;
case 192:
#line 658 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.StructureReturn; }
  break;
case 193:
#line 659 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.StructureReturn; }
  break;
case 194:
#line 660 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoAlias; }
  break;
case 195:
#line 661 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Byval; }
  break;
case 196:
#line 662 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Byval; }
  break;
case 197:
#line 666 "Iril/IR/IR.jay"
  {
        yyVal = ParameterAttributes.Dereferenceable;
    }
  break;
case 198:
#line 670 "Iril/IR/IR.jay"
  {
        yyVal = ParameterAttributes.Align8;
    }
  break;
case 212:
#line 705 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.Equal; }
  break;
case 213:
#line 706 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.NotEqual; }
  break;
case 214:
#line 707 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThan; }
  break;
case 215:
#line 708 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThanOrEqual; }
  break;
case 216:
#line 709 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThan; }
  break;
case 217:
#line 710 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThanOrEqual; }
  break;
case 218:
#line 711 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThan; }
  break;
case 219:
#line 712 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThanOrEqual; }
  break;
case 220:
#line 713 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThan; }
  break;
case 221:
#line 714 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThanOrEqual; }
  break;
case 222:
#line 718 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.True; }
  break;
case 223:
#line 719 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.False; }
  break;
case 224:
#line 720 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.Ordered; }
  break;
case 225:
#line 721 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedEqual; }
  break;
case 226:
#line 722 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedNotEqual; }
  break;
case 227:
#line 723 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThan; }
  break;
case 228:
#line 724 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThanOrEqual; }
  break;
case 229:
#line 725 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThan; }
  break;
case 230:
#line 726 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThanOrEqual; }
  break;
case 231:
#line 727 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.Unordered; }
  break;
case 232:
#line 728 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedEqual; }
  break;
case 233:
#line 729 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedNotEqual; }
  break;
case 234:
#line 730 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThan; }
  break;
case 235:
#line 731 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThanOrEqual; }
  break;
case 236:
#line 732 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThan; }
  break;
case 237:
#line 733 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThanOrEqual; }
  break;
case 238:
#line 737 "Iril/IR/IR.jay"
  { yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]); }
  break;
case 242:
#line 747 "Iril/IR/IR.jay"
  { yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]); }
  break;
case 243:
#line 751 "Iril/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 244:
#line 755 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 245:
#line 759 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 246:
#line 763 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 247:
#line 767 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 248:
#line 771 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 249:
#line 775 "Iril/IR/IR.jay"
  {
        yyVal = new VectorConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 250:
#line 779 "Iril/IR/IR.jay"
  {
        yyVal = new ArrayConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 251:
#line 783 "Iril/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 252:
#line 787 "Iril/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-2+yyTop]);
    }
  break;
case 253:
#line 791 "Iril/IR/IR.jay"
  {
        yyVal = new AddrSpaceCastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 255:
#line 799 "Iril/IR/IR.jay"
  { yyVal = NullConstant.Null; }
  break;
case 256:
#line 800 "Iril/IR/IR.jay"
  { yyVal = new FloatConstant ((double)yyVals[0+yyTop]); }
  break;
case 257:
#line 801 "Iril/IR/IR.jay"
  { yyVal = new IntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 258:
#line 802 "Iril/IR/IR.jay"
  { yyVal = new HexIntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 259:
#line 803 "Iril/IR/IR.jay"
  { yyVal = BooleanConstant.True; }
  break;
case 260:
#line 804 "Iril/IR/IR.jay"
  { yyVal = BooleanConstant.False; }
  break;
case 261:
#line 805 "Iril/IR/IR.jay"
  { yyVal = UndefinedConstant.Undefined; }
  break;
case 262:
#line 806 "Iril/IR/IR.jay"
  { yyVal = ZeroConstant.Zero; }
  break;
case 263:
#line 807 "Iril/IR/IR.jay"
  { yyVal = new BytesConstant ((Symbol)yyVals[0+yyTop]); }
  break;
case 264:
#line 814 "Iril/IR/IR.jay"
  {
        yyVal = new LabelValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 265:
#line 821 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 266:
#line 825 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue (VoidType.Void, VoidValue.Void);
    }
  break;
case 267:
#line 832 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 268:
#line 839 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 269:
#line 843 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 270:
#line 850 "Iril/IR/IR.jay"
  {
        yyVal = new TypedConstant ((LType)yyVals[-1+yyTop], (Constant)yyVals[0+yyTop]);
    }
  break;
case 272:
#line 858 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 273:
#line 865 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 274:
#line 869 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 276:
#line 880 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Value)yyVals[0+yyTop]);
    }
  break;
case 277:
#line 884 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 278:
#line 891 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Block)yyVals[0+yyTop]);
    }
  break;
case 279:
#line 895 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Block)yyVals[0+yyTop]);
    }
  break;
case 280:
#line 902 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 281:
  case_281();
  break;
case 282:
#line 912 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 283:
#line 919 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 284:
#line 923 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-2+yyTop], (Assignment)yyVals[-1+yyTop]);
    }
  break;
case 285:
#line 927 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (Assignment)yyVals[0+yyTop]);
    }
  break;
case 286:
#line 931 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (Assignment)yyVals[-1+yyTop]);
    }
  break;
case 287:
#line 938 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Assignment)yyVals[0+yyTop]);
    }
  break;
case 288:
#line 942 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 289:
#line 949 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[0+yyTop]);
    }
  break;
case 290:
#line 953 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[0+yyTop]);
    }
  break;
case 291:
#line 957 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 292:
#line 961 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-3+yyTop], (Instruction)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[0+yyTop]);
    }
  break;
case 294:
#line 969 "Iril/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 295:
#line 970 "Iril/IR/IR.jay"
  { yyVal = Enumerable.Empty<Argument> (); }
  break;
case 296:
#line 977 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Argument)yyVals[0+yyTop]);
    }
  break;
case 297:
#line 981 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Argument)yyVals[0+yyTop]);
    }
  break;
case 298:
#line 988 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 299:
#line 992 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], ParameterAttributes.NonNull);
    }
  break;
case 300:
#line 996 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 301:
#line 1000 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[0+yyTop]), (ParameterAttributes)0);
    }
  break;
case 302:
#line 1004 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-2+yyTop]), (ParameterAttributes)0);
    }
  break;
case 303:
#line 1008 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-3+yyTop]), (ParameterAttributes)0);
    }
  break;
case 305:
#line 1016 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]);
    }
  break;
case 306:
#line 1020 "Iril/IR/IR.jay"
  {
        yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 307:
#line 1024 "Iril/IR/IR.jay"
  {
        yyVal = new SymbolValue ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 308:
#line 1028 "Iril/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 309:
#line 1032 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 310:
#line 1036 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 311:
#line 1040 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 312:
#line 1044 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 320:
#line 1064 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((PhiValue)yyVals[0+yyTop]);
    }
  break;
case 321:
#line 1068 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (PhiValue)yyVals[0+yyTop]);
    }
  break;
case 322:
#line 1074 "Iril/IR/IR.jay"
  {
        yyVal = new PhiValue ((Value)yyVals[-3+yyTop], (Value)yyVals[-1+yyTop]);
    }
  break;
case 323:
#line 1081 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 324:
#line 1085 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 325:
#line 1092 "Iril/IR/IR.jay"
  {
        yyVal = new SwitchCase ((TypedConstant)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 333:
#line 1112 "Iril/IR/IR.jay"
  { yyVal = AtomicConstraint.SequentiallyConsistent; }
  break;
case 334:
#line 1119 "Iril/IR/IR.jay"
  {
        yyVal = new InlineAssemblyValue ((string)yyVals[-2+yyTop], (string)yyVals[0+yyTop]);
    }
  break;
case 335:
#line 1126 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment (LocalSymbol.None, (Instruction)yyVals[0+yyTop]);
    }
  break;
case 336:
#line 1130 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 337:
#line 1137 "Iril/IR/IR.jay"
  {
        yyVal = new UnconditionalBrInstruction ((LabelValue)yyVals[0+yyTop]);
    }
  break;
case 338:
#line 1141 "Iril/IR/IR.jay"
  {
        yyVal = new ConditionalBrInstruction ((Value)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 339:
#line 1145 "Iril/IR/IR.jay"
  {
        yyVal = new ResumeInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 340:
#line 1149 "Iril/IR/IR.jay"
  {
        yyVal = new RetInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 341:
#line 1153 "Iril/IR/IR.jay"
  {
        yyVal = new SwitchInstruction ((TypedValue)yyVals[-5+yyTop], (LabelValue)yyVals[-3+yyTop], (List<SwitchCase>)yyVals[-1+yyTop]);
    }
  break;
case 342:
#line 1157 "Iril/IR/IR.jay"
  {
        yyVal = UnreachableInstruction.Unreachable;
    }
  break;
case 344:
#line 1165 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 345:
#line 1169 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 346:
#line 1173 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 347:
#line 1177 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 348:
#line 1184 "Iril/IR/IR.jay"
  {
        yyVal = false;
    }
  break;
case 349:
#line 1188 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 350:
#line 1195 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 351:
#line 1199 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 352:
#line 1203 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-8+yyTop], (Value)yyVals[-7+yyTop], (Value)yyVals[-4+yyTop], isAtomic: true);
    }
  break;
case 353:
#line 1207 "Iril/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-3+yyTop], (int)(BigInteger)yyVals[0+yyTop], numElements: (TypedValue)null);
    }
  break;
case 354:
#line 1211 "Iril/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-5+yyTop], (int)(BigInteger)yyVals[0+yyTop], numElements: (TypedValue)yyVals[-3+yyTop]);
    }
  break;
case 355:
#line 1215 "Iril/IR/IR.jay"
  {
        yyVal = new AndInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 356:
#line 1219 "Iril/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 357:
#line 1223 "Iril/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 358:
#line 1227 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 359:
#line 1231 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 360:
#line 1235 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 361:
#line 1239 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 362:
#line 1243 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 363:
#line 1247 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 364:
#line 1251 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 365:
#line 1255 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 366:
#line 1259 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 367:
#line 1263 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 368:
#line 1267 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 369:
#line 1271 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 370:
#line 1275 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 371:
#line 1279 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 372:
#line 1283 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 373:
#line 1287 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 374:
#line 1291 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 375:
#line 1295 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 376:
#line 1299 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 377:
#line 1303 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 378:
#line 1307 "Iril/IR/IR.jay"
  {
        yyVal = new ExtractElementInstruction ((TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 379:
#line 1311 "Iril/IR/IR.jay"
  {
        yyVal = new ExtractValueInstruction ((TypedValue)yyVals[-2+yyTop], (List<Value>)yyVals[0+yyTop]);
    }
  break;
case 380:
#line 1315 "Iril/IR/IR.jay"
  {
        yyVal = new FaddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 381:
#line 1319 "Iril/IR/IR.jay"
  {
        yyVal = new FcmpInstruction ((FcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 382:
#line 1323 "Iril/IR/IR.jay"
  {
        yyVal = new FdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 383:
#line 1327 "Iril/IR/IR.jay"
  {
        yyVal = new FenceInstruction ((AtomicConstraint)yyVals[0+yyTop]);
    }
  break;
case 384:
#line 1331 "Iril/IR/IR.jay"
  {
        yyVal = new FmulInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 385:
#line 1335 "Iril/IR/IR.jay"
  {
        yyVal = new FpextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 386:
#line 1339 "Iril/IR/IR.jay"
  {
        yyVal = new FptouiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 387:
#line 1343 "Iril/IR/IR.jay"
  {
        yyVal = new FptosiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 388:
#line 1347 "Iril/IR/IR.jay"
  {
        yyVal = new FptruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 389:
#line 1351 "Iril/IR/IR.jay"
  {
        yyVal = new FsubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 390:
#line 1355 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 391:
#line 1359 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 392:
#line 1363 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 393:
#line 1367 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 394:
#line 1371 "Iril/IR/IR.jay"
  {
        yyVal = new IcmpInstruction ((IcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 395:
#line 1375 "Iril/IR/IR.jay"
  {
        yyVal = new InsertElementInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 396:
#line 1379 "Iril/IR/IR.jay"
  {
        yyVal = new InsertValueInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<Value>)yyVals[0+yyTop]);
    }
  break;
case 397:
#line 1383 "Iril/IR/IR.jay"
  {
        yyVal = new InttoptrInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 398:
#line 1387 "Iril/IR/IR.jay"
  {
        yyVal = new LandingPadInstruction ((LType)yyVals[-1+yyTop]);
    }
  break;
case 399:
#line 1391 "Iril/IR/IR.jay"
  {
        yyVal = new LandingPadInstruction ((LType)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 400:
#line 1395 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false, isAtomic: false);
    }
  break;
case 401:
#line 1399 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false, isAtomic: true);
    }
  break;
case 402:
#line 1403 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true, isAtomic: false);
    }
  break;
case 403:
#line 1407 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true, isAtomic: true);
    }
  break;
case 404:
#line 1411 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-6+yyTop], (TypedValue)yyVals[-4+yyTop], isVolatile: true, isAtomic: true);
    }
  break;
case 405:
#line 1415 "Iril/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 406:
#line 1419 "Iril/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 407:
#line 1423 "Iril/IR/IR.jay"
  {
        yyVal = new OrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 408:
#line 1427 "Iril/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 409:
#line 1431 "Iril/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 410:
#line 1435 "Iril/IR/IR.jay"
  {
        yyVal = new PhiInstruction ((LType)yyVals[-1+yyTop], (List<PhiValue>)yyVals[0+yyTop]);
    }
  break;
case 411:
#line 1439 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 412:
#line 1443 "Iril/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 413:
#line 1447 "Iril/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 414:
#line 1451 "Iril/IR/IR.jay"
  {
        yyVal = new SelectInstruction ((LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 415:
#line 1455 "Iril/IR/IR.jay"
  {
        yyVal = new SextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 416:
#line 1459 "Iril/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 417:
#line 1463 "Iril/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 418:
#line 1467 "Iril/IR/IR.jay"
  {
        yyVal = new ShuffleVectorInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 419:
#line 1471 "Iril/IR/IR.jay"
  {
        yyVal = new SitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 420:
#line 1475 "Iril/IR/IR.jay"
  {
        yyVal = new SremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 421:
#line 1479 "Iril/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: false);
    }
  break;
case 422:
#line 1483 "Iril/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: true);
    }
  break;
case 423:
#line 1487 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 424:
#line 1491 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 425:
#line 1495 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-8+yyTop], (Value)yyVals[-7+yyTop], (Value)yyVals[-4+yyTop], isAtomic: true);
    }
  break;
case 426:
#line 1499 "Iril/IR/IR.jay"
  {
        yyVal = new TruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 427:
#line 1503 "Iril/IR/IR.jay"
  {
        yyVal = new UdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 428:
#line 1507 "Iril/IR/IR.jay"
  {
        yyVal = new UitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 429:
#line 1511 "Iril/IR/IR.jay"
  {
        yyVal = new UremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 430:
#line 1515 "Iril/IR/IR.jay"
  {
        yyVal = new XorInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 431:
#line 1519 "Iril/IR/IR.jay"
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
#line 83 "Iril/IR/IR.jay"
{
        var f = (FunctionDefinition)yyVals[0+yyTop];
        module.FunctionDefinitions[f.Symbol] = f;
    }

void case_10()
#line 88 "Iril/IR/IR.jay"
{
        var f = (FunctionDeclaration)yyVals[0+yyTop];
        module.FunctionDeclarations[f.Symbol] = f;
    }

void case_11()
#line 93 "Iril/IR/IR.jay"
{
        var g = (GlobalVariable)yyVals[0+yyTop];
        module.AddGlobalVariable(g);
    }

void case_16()
#line 108 "Iril/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = m;
    }

void case_18()
#line 117 "Iril/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = m;
    }

void case_60()
#line 274 "Iril/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = NewSyms (t.Item1, t.Item2);
    }

void case_61()
#line 279 "Iril/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = SymsAdd (yyVals[-2+yyTop], t.Item1, t.Item2);
    }

void case_130()
#line 484 "Iril/IR/IR.jay"
{
        var h = (Tuple<object, object>)yyVals[-6+yyTop];
        yyVal = new FunctionDefinition ((LType)h.Item2, (GlobalSymbol)yyVals[-5+yyTop], (IEnumerable<Parameter>)yyVals[-4+yyTop], (List<Block>)yyVals[-1+yyTop], isExternal: (bool)h.Item1);
    }

void case_131()
#line 489 "Iril/IR/IR.jay"
{
        var h = (Tuple<object, object>)yyVals[-7+yyTop];
        yyVal = new FunctionDefinition ((LType)h.Item2, (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop], isExternal: (bool)h.Item1, (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }

void case_281()
#line 904 "Iril/IR/IR.jay"
{
        /* Named basic-block label (e.g. `entry:`). Record it as %<name> so*/
        /* `br label %<name>` targets resolve to this block.*/
        yyVal = ((Block)yyVals[0+yyTop]).WithSymbol ((LocalSymbol)Symbol.Intern ("%" + ((Symbol)yyVals[-2+yyTop]).Text));
    }

#line default
   static readonly short [] yyLhs  = {              -1,
    0,    1,    1,    2,    2,    2,    2,    2,    2,    2,
    2,    2,    2,    2,    2,    2,    2,    2,    6,    6,
    6,    6,    6,    6,    6,    6,    6,    6,    6,    6,
    6,    6,    6,    6,    6,    6,    6,    6,    6,    6,
    6,    6,    6,    6,    6,    6,   11,   11,   14,   10,
   10,   10,   10,   10,   10,   10,   10,   18,   16,    9,
    9,   19,   19,   19,   19,   19,   20,   23,   23,   24,
   25,   25,   25,   25,   25,   25,   17,   17,    8,    8,
    8,    8,    8,   27,   27,   27,    7,    7,   29,   29,
   29,   29,   29,   29,   29,   29,   29,   29,   29,   29,
   29,    3,    3,    3,   30,   30,   31,   31,   12,   12,
   12,   12,   12,   12,   12,   12,   12,   12,   12,   12,
   12,   12,   34,   33,   33,   32,   32,   35,   35,    4,
    4,   38,   38,   38,   38,   38,   38,   38,   38,   38,
   38,   38,   36,   36,   36,   36,   36,   43,   43,   43,
   43,   43,   43,   43,   41,   47,   47,    5,    5,    5,
    5,    5,   48,   48,   48,   37,   37,   49,   49,   50,
   50,   50,   50,   50,   50,   44,   44,   42,   42,   42,
   42,   42,   42,   42,   42,   42,   42,   42,   42,   42,
   42,   42,   42,   42,   42,   42,   42,   42,   51,   51,
   52,   52,   15,   15,   15,   15,   45,   45,   40,   40,
   53,   54,   54,   54,   54,   54,   54,   54,   54,   54,
   54,   55,   55,   55,   55,   55,   55,   55,   55,   55,
   55,   55,   55,   55,   55,   55,   55,   56,   13,   13,
   57,   57,   57,   57,   57,   57,   57,   57,   57,   57,
   57,   57,   57,   60,   21,   21,   21,   21,   21,   21,
   21,   21,   21,   61,   28,   28,   62,   59,   59,   26,
   63,   63,   58,   58,   64,   65,   65,   39,   39,   66,
   66,   66,   67,   67,   67,   67,   68,   68,   70,   70,
   70,   70,   72,   73,   73,   74,   74,   75,   75,   75,
   75,   75,   75,   76,   76,   76,   76,   76,   76,   76,
   76,   76,   22,   22,   77,   77,   77,   77,   77,   78,
   78,   79,   80,   80,   81,   82,   82,   83,   83,   46,
   46,   46,   84,   85,   69,   69,   86,   86,   86,   86,
   86,   86,   86,   87,   87,   87,   87,   88,   88,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,
  };
   static readonly short [] yyLen = {           2,
    1,    1,    2,    3,    4,    4,    4,    4,    1,    1,
    1,    6,    5,    5,    6,    6,    7,    7,    6,    5,
    8,    6,    9,    6,    6,    9,   10,   13,    9,   10,
   10,   10,   10,    7,   11,    6,    9,    9,   12,    9,
   10,   11,    9,   10,    8,    5,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    3,    3,    3,    3,    6,    5,    1,    1,    3,    1,
    1,    1,    1,    1,    1,    1,    2,    3,    1,    2,
    3,    3,    3,    1,    1,    1,    1,    2,    1,    1,
    1,    1,    1,    1,    1,    3,    1,    1,    1,    1,
    4,    2,    3,    5,    1,    3,    1,    1,    1,    1,
    1,    1,    1,    1,    3,    4,    3,    5,    1,    2,
    5,    5,    4,    0,    4,    1,    3,    1,    1,    7,
    8,    1,    2,    4,    3,    5,    4,    1,    3,    2,
    4,    3,    2,    3,    3,    4,    4,    1,    1,    1,
    1,    2,    3,    2,    2,    1,    2,    4,    5,    6,
    6,    7,    1,    2,    1,    3,    2,    1,    3,    1,
    2,    2,    3,    1,    1,    1,    2,    1,    1,    4,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    4,    1,    1,    4,    4,    2,    1,    3,
    1,    1,    2,    3,    2,    1,    1,    1,    1,    2,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    6,    9,   10,    8,    6,    6,    3,    3,
    3,    5,    6,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    2,    2,    1,    2,    1,    3,    2,
    1,    2,    1,    3,    1,    1,    3,    1,    2,    3,
    3,    1,    2,    3,    1,    2,    1,    2,    1,    2,
    3,    4,    1,    3,    2,    1,    3,    2,    3,    3,
    2,    4,    5,    1,    1,    1,    1,    6,    9,   10,
    6,    6,    1,    3,    1,    1,    2,    2,    2,    1,
    3,    5,    1,    2,    3,    1,    2,    1,    1,    1,
    1,    1,    1,    5,    1,    3,    2,    7,    2,    2,
    7,    1,    1,    8,    9,    9,   10,    0,    1,    5,
    6,   11,    5,    7,    5,    5,    6,    4,    4,    5,
    5,    6,    6,    7,    5,    5,    6,    6,    7,    6,
    7,    5,    6,    7,    7,    8,    6,    4,    4,    6,
    7,    6,    2,    6,    4,    4,    4,    4,    6,    6,
    7,    8,    7,    6,    6,    6,    4,    3,    4,    7,
    8,    8,    9,   10,    5,    6,    5,    5,    6,    3,
    4,    5,    6,    8,    4,    5,    6,    6,    4,    5,
    7,    8,    5,    6,   11,    4,    5,    4,    5,    5,
    4,
  };
   static readonly short [] yyDefRed = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    2,    9,   10,   11,    0,    0,    0,    0,    0,    0,
    0,    0,  108,  119,  111,  112,  113,  114,  110,    0,
  148,   51,   52,   53,   54,   55,   56,   57,  330,  189,
  190,  191,    0,   50,    0,  178,  179,  187,  186,  188,
    0,  207,  208,    0,  184,  185,    0,  331,  332,  181,
  182,  183,    0,    0,    0,  109,    0,    0,    0,    0,
    0,  149,  150,    0,    0,    0,    3,    0,    0,    0,
  176,    0,    4,    0,    0,  201,  202,   47,   48,   58,
   49,   59,    0,    0,    0,    0,    0,    0,    0,    0,
  206,    0,    0,    0,    0,    0,    0,  120,    0,    0,
    0,  198,    0,  102,    0,    0,    0,    0,    0,    0,
    0,  154,    0,    0,    0,  194,    0,    0,    0,   77,
    0,    0,    0,    0,    0,    0,    0,    0,  177,    5,
    6,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  205,    0,    8,    0,    7,    0,
    0,    0,    0,    0,    0,    0,    0,  199,    0,  103,
    0,    0,    0,    0,  153,    0,  129,  115,    0,    0,
  126,    0,    0,   78,    0,  174,  175,  167,    0,    0,
  168,  211,    0,    0,    0,  209,    0,    0,    0,    0,
    0,    0,    0,    0,  257,  258,  256,  259,  260,  261,
  255,  238,  242,  263,  262,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  241,  239,  240,    0,    0,    0,
    0,    0,    0,    0,    0,  204,    0,    0,    0,    0,
   60,    0,    0,    0,   86,   85,   14,    0,    0,   79,
   84,    0,  197,  193,  196,  180,    0,    0,    0,    0,
    0,    0,  116,    0,    0,    0,  100,   99,   91,   89,
   90,   92,   93,   94,   95,   13,    0,   87,  171,    0,
  166,    0,    0,    0,    0,    0,    0,    0,  140,  210,
    0,    0,    0,    0,  159,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  268,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   16,    0,    0,    0,   80,   15,
    0,  265,  125,  200,  121,  104,  122,  118,  127,    0,
    0,   12,   88,  173,  169,    0,    0,  135,    0,    0,
    0,    0,    0,    0,    0,    0,  342,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  278,  282,    0,    0,  287,
    0,  335,  343,    0,  142,  155,    0,  160,    0,    0,
  161,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  251,    0,    0,    0,  249,  250,    0,    0,
    0,    0,    0,    0,    0,    0,   73,   76,    0,   71,
    0,   62,   74,    0,   68,   70,   75,   72,   63,   64,
   61,   18,   17,   83,   82,   81,   96,  316,    0,  315,
    0,  313,  137,    0,    0,    0,    0,  340,    0,    0,
  337,    0,    0,    0,    0,  339,  328,  329,    0,    0,
  326,  349,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  333,  383,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  212,  213,  214,  215,  216,  217,
  218,  219,  220,  221,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  130,  279,    0,  288,    0,    0,    0,
  141,  162,   45,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  269,    0,    0,   21,    0,    0,
    0,    0,    0,    0,    0,    0,  270,    0,  319,  317,
  318,  101,    0,  136,  280,    0,  336,  281,  264,    0,
    0,  293,    0,    0,    0,    0,    0,    0,  327,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  222,  223,  234,  235,  236,  237,  225,  227,
  228,  229,  230,  226,  224,  232,  233,  231,    0,    0,
    0,  320,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  398,    0,    0,  131,   26,    0,   40,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  252,
    0,   23,    0,    0,    0,    0,   37,    0,   66,    0,
   69,  314,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  378,    0,    0,  275,  276,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  399,    0,    0,    0,    0,
    0,    0,  248,  243,  247,  253,    0,    0,   31,    0,
    0,   65,    0,    0,    0,  295,    0,    0,  296,    0,
    0,    0,    0,  350,    0,    0,  423,    0,    0,  408,
    0,    0,  427,    0,  412,    0,  429,  420,  416,    0,
    0,  405,    0,  356,  355,  407,  430,    0,    0,    0,
    0,  353,    0,    0,    0,    0,  254,  267,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  321,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  271,
    0,  273,    0,    0,    0,    0,    0,  323,    0,    0,
  298,    0,  294,    0,    0,    0,    0,    0,  351,  380,
  424,  389,  409,  384,  413,  382,  417,  406,  357,  395,
  418,  277,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  394,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  272,  246,    0,   39,  338,    0,  341,
  324,    0,  305,  306,  307,    0,    0,    0,    0,  304,
  300,  299,  297,    0,    0,    0,    0,  354,    0,    0,
    0,    0,  400,    0,  421,    0,    0,    0,    0,    0,
  381,  322,    0,  334,    0,    0,    0,    0,    0,   28,
    0,  244,  274,  325,  302,    0,    0,    0,    0,    0,
  344,    0,    0,    0,  402,    0,    0,  401,  422,    0,
    0,    0,  414,    0,  245,  303,    0,    0,    0,    0,
    0,  345,  346,    0,    0,  403,    0,    0,    0,    0,
    0,    0,    0,  347,  404,    0,    0,    0,    0,    0,
    0,    0,  352,  425,    0,    0,  312,  308,  311,    0,
    0,    0,    0,    0,  309,  310,
  };
  protected static readonly short [] yyDgoto  = {             9,
   10,   11,   66,   12,   13,   14,  277,  248,  240,   67,
   95,  249,  612,   96,  293,   98,   75,   99,  241,  452,
  225,  471,  454,  455,  456,  457,  250,  910,  278,  116,
  117,  180,  123,  101,  181,   15,  134,  194,  405,  294,
  289,   81,   71,   82,   72,   73,   16,  295,  190,  191,
  169,  102,  196,  555,  689,  226,  227,  911,  309,  878,
  481,  778,  912,  769,  770,  406,  407,  408,  409,  410,
  411,  613,  737,  838,  839,  991,  472,  691,  692,  917,
  918,  490,  491,  527,  696,  412,  413,  493,
  };
  protected static readonly short [] yySindex = {          359,
   -8, -100,   38,   71,  125, 1204, -207, -132,    0,  359,
    0,    0,    0,    0,  -71, 4174,  -52,  156,  193,  950,
   18,  -10,    0,    0,    0,    0,    0,    0,    0, -124,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  225,    0,  232,    0,    0,    0,    0,    0,
  234,    0,    0,   23,    0,    0,  242,    0,    0,    0,
    0,    0, 2142,  -76,   46,    0, -130, -124,  268, 5836,
 3886,    0,    0,   35,   40,  273,    0,  270, 4217,  -16,
    0, 4217,    0,   87,   97,    0,    0,    0,    0,    0,
    0,    0,  330,   32, 5836,   32,  -75,   45,   45,   55,
    0, -124,  -25,  344,    9,  172,  346,    0,  140, 5836,
 5836,    0,  129,    0, -124,   70,  268,  152, 5836,  164,
 -248,    0,  390, 5355,  268,    0, 5836,  268, 4217,    0,
  171,  327, 5329, -113,   -2, 4217,  270,    1,    0,    0,
    0,  200, 5836,  -75,  -75, 4381, 5836,  -75,  -75, 5836,
 5836,  -75, 5836,  -75,    0,  169,    0,  368,    0, -166,
  458,  387, 4661,  254,  480,  -39,  -34,    0,   90,    0,
 5836, 5836,  117, 5836,    0,  145,    0,    0, -124,  128,
    0,  268,  268,    0, 1025,    0,    0,    0, 5996,  247,
    0,    0,  217,  -61, -141,    0,  270,   14, -113,  270,
  493, 2180, 5836, 5836,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  -29,  503,  507,  510,  511,
 5866, 5889, 5866,  509,    0,    0,    0,  -24, 5836, 5836,
 4381, 4381, 5836, 4381, 5836,    0,  499,  502,  505,  249,
    0, -166, 5717,    0,    0,    0,    0,   -5, 4381,    0,
    0,  525,    0,    0,    0,    0,  292, -124,  -41,  512,
  -64,  312,    0, 5575,  518,  533,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, 1426,    0,    0, 6015,
    0, 5560, -122, 5871,  -60,  320, 5866,  332,    0,    0,
 -113,  270,  217,  217,    0, -113,    0,  203,  547, 4026,
 -124,  -18, 5836, 5866, 5866, 5866, 5866,    0,  118, 5690,
   81,  107,  213,  214,    6, -124,  552,  558, 4381,  562,
 4381, 5616, 5649,  767,    0, -166,  278,    4,    0,    0,
 5743,    0,    0,    0,    0,    0,    0,    0,    0,  348,
 5682,    0,    0,    0,    0,  349,  355,    0,  560,  555,
  561, 5866,  -90, 5866, 3836, 5866,    0,  818,  199,  818,
  199,  818,  199, 5836, 2649,  199, 5836, 5836,  818, 3007,
 3365, 5836, 5836, 5836, 5866, 5866, 5866, 5866, 5866, 5836,
  -48, 5372,  291, -188, 4431, 5866, 5866, 5866, 5866, 5866,
 5866, 5866, 5866, 5866, 5866, 5866, 5866, 1977,  199, 5836,
  199, 3836,  202, 5836, 4490,    0,    0, 8924, -207,    0,
 -207,    0,    0, 5871,    0,    0,  316,    0, -113,  217,
    0,  378, -229,  258,  602,  608, 5836,    8,  252,  262,
  276,  279,    0, 5866, 4381,  123,    0,    0,  411,  413,
  293,  298,  300,  638, -206,  642,    0,    0,  647,    0,
  209,    0,    0,  565,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, -127,    0,
  282,    0,    0,  316, 8924, 9423, 8924,    0,  419, 4424,
    0,  651, 1134, 4217, 4217,    0,    0,    0, 4381,  818,
    0,    0, 5836, 4381,  818, 5836, 4381,  818, 5836, 4381,
 5836, 4381, 5836, 4381, 4381, 4381,  818, 5836, 4381, 5836,
 4381, 4381, 4381, 4381,  654,  655,  656,  657,  658,   11,
 5836, 5389,   17, 5866,  659,    0,    0, 5836, 5836, 5836,
 4905,   22,  314,  319,  322,  323,  324,  325,  326,  328,
  329,  335,  336,  337,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, 5836, 2391,  -66, 5836,  342, 5836,
 4217,  801, -256,    0,    0, -207,    0,   40,   40, 4615,
    0,    0,    0,  450,  460,  471, -199, 5836,   24, 5866,
 5836, 5836, 5836, 5836,    0,  667, -207,    0,  474,  476,
  477,  361,  481,  488,  365, 5472,    0, 5649,    0,    0,
    0,    0, 5682,    0,    0, -207,    0,    0,    0,  707,
  486,    0,  714, 1134, 1134, 4217,  712, 4381,    0, 4381,
  715, 4381, 4381,  716, 4381, 4381,  717, 4381,  718, 4381,
  719,  720,  730, 4381, 4381,  731, 4381,  740,  755,  758,
  761, 5866, 5866, 5866,  767, 5866,  255,   26, 5836,   27,
 5836,  762, 5836, 4381, 4381,   28, 5836,   36, 5866, 5836,
 5836, 5836, 5836, 5836, 5836, 5836, 5836, 5836, 5836, 5836,
 5836, 4381,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 5836, 4424,
  766,    0, 4381,  383,  714,  714, 1134, 1134, 5836, 5836,
  342, 5836, 4217,    0, 5866,   40,    0,    0, -207,    0,
  498,  556,   41, 5866,  770,  -15,  -13,  -11,   -7,    0,
   40,    0, -207, -207,  567,  771,    0,  568,    0,  286,
    0,    0,   40,  486,  727, 5536,  429,  714,  714, 1134,
 4424,  782,  783, 4424,  786,  787, 4424,  788,  789, 4424,
  790, 4424,  793, 4424, 4424, 4424,  796,  797, 4424,  799,
 4424, 4424, 4424, 4424,    0,  802,  803,    0,    0,  804,
  807,  595,  811, 5836,   50, 5836, 4381,  816, 5836,  819,
  820,  823, 5866,   51, 5866,  824, -124, -124, -124, -124,
 -124, -124, -124, -124, -124, -124, -124, -124,  826, 4381,
  831,  791,  835,  621,  217,  217,  714,  714, 1134, 1134,
  714,  714, 1134, 1134, 4217,    0,   40,  839, -207, 5866,
  840, 3850,    0,    0,    0,    0,   40,   40,    0,  506,
 -207,    0,  842, 5836, 5771,    0, 3643,  307,    0,  486,
  472,  501,  714,    0, 4424, 4424,    0, 4424, 4424,    0,
 4424, 4424,    0, 4424,    0, 4424,    0,    0,    0, 4424,
 4424,    0, 4424,    0,    0,    0,    0, 5866, 5866,  767,
  767,    0,  508,  844, 5836,  849,    0,    0,  513,  851,
  515, 5836, 5836,  853, 5866,  855, 3850, 4424,  856, 4424,
    0, 5866,  857,  217,  217,  217,  217,  714,  714,  217,
  217,  714,  714, 1134,  521,   40,  859, 3850, 5866,    0,
  386,    0,  648,   40,  486,  863, 5794,    0,  870, 2763,
    0, 3786,    0, 5828,  583,  486,  486,  523,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  804,  660,  534,  -38,  535,  664,  542,  669,
 4381, 4381, 3850,  885, 3850,  887,    0, 4424,  821,  888,
  673,  217,  217,  217,  217,  217,  217,  217,  217,  714,
  677, 3850,  402,    0,    0, 3850,    0,    0,  486,    0,
    0, 5506,    0,    0,    0,  566,  898,  902,  903,    0,
    0,    0,    0,  486,  618,  630,  486,    0,  705,  935,
  599,  725,    0,  741,    0,  666,  668,  887, 3850,  887,
    0,    0, 5866,    0,  217,  217,  217,  217,  217,    0,
  420,    0,    0,    0,    0,  432,   -9, 5866, 5866, 5866,
    0,  486,  486,  672,    0,  607,  746,    0,    0,  960,
  961,  887,    0,  217,    0,    0,  966, 5836,  617,  620,
  622,    0,    0,  486,  754,    0,  632,  633, 5836,   52,
 5836, 5836, 5836,    0,    0,  759,  774,   53, 5866,   -1,
    3,    5,    0,    0, 5866,  974,    0,    0,    0,  975,
 3850, 3850,  439,  441,    0,    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0, 4267,    0,    0, 1033,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  723,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 1341,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, 1535,    0,    0,    0,    0,    0,
 1936,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, 3929,  190,  763,    0,
    0,    0,    0,    0, 4310,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  723,    0,  723,    0,  723,  723,    0,
    0,  548,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  256,    0,    0,    0,    0,    0,
 4131,    0,    0,    0,  764,    0,    0,  765,    0,    0,
    0,    0,    0,  723,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  113,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  475,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  113,  113,    0,    0,    0,
    0,    0,    0,    0,    0, 1742,    0,    0,  400,    0,
    0,  778,  780,    0,    0,    0,    0,    0,  636,    0,
    0,    0,  -54,    0,  -46,    0,    0,    0, 1136,    0,
    0, 1038,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  866,    0,    0,
  113,  113,    0,  113,    0,    0,    0,    0,    0,    0,
    0,    0,    0, 2143,    0,    0,    0,    0,  113,    0,
    0,    0,    0,    0,    0,    0,    0,  382,  113,    0,
  113,    0,    0,    0, 2026, 2144,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  459,
    0,    0,  -30,    0,    0,    0,    0,    0,    0,    0,
  723,    0,  245,  890,    0,  723, 1601,    0, 1097,  113,
  880,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  999, 1009,    0,    0,  113, 1297,
  113,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 5918,    0,
 5918,    0, 5918,    0,    0, 5918,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 2410,    0,
 5918,    0,    0,    0,    0,    0,    0,    0, 4748,    0,
 9029,    0,    0,    0,    0,    0,  -22,    0,  723, 1537,
    0,    0,    0,    0,    0,    0,    0,  113,    0,    0,
    0,    0,    0,    0,  256,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 1611,    0,    0,   67,    0,
  113,    0,    0,  468,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  113,    0,
    0,    0,    0,  -21,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  113,    0,
    0,    0,    0,  113,    0,    0,  113,    0,    0,  113,
    0,  113,    0,  113,  113,  113,    0,    0,  113,    0,
  113,  113,  113,  113,    0,    0,    0,    0,    0,  113,
    0,    0,  113,    0,    0,    0,    0,    0,    0,    0,
    0,  113,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  113,    0,    0,    0,
    0,    0,  113,    0,    0, 4873,    0, 5006, 9134,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  113,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 9239,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  113,    0,  113,
    0,  113,  113,    0,  113,  113,    0,  113,    0,  113,
    0,    0,    0,  113,  113,    0,  113,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  113,    0,  113,
    0,    0,    0,  113,  113,  113,    0,  113,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  113,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
 6119,    0,  113,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 5131,    0,    0, 1621,    0,
    0,    0,  113,    0,    0,  113,  113,  113,  113,    0,
 1666,    0,    0,    0,    0, 1676,    0,    0,    0,    0,
    0,    0, 9344,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 6227,
    0,    0,    0,    0,  113,    0,  113,    0,    0,    0,
    0,    0,    0,  113,    0,    0,  598, 2251, 2359, 2501,
 2609, 2717, 2859, 2967, 3075, 3217, 3325, 3433,    0,  113,
    0,    0,    0,    0, 6335,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, 1888, 1959, 2009,    0,
    0,    0,    0,    0,    0,    0, 2037, 2046,    0,    0,
 2070,    0,    0,    0,    0,    0,  113,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0, 6443, 6551, 6659, 6767,    0,    0, 6875,
    0,    0,    0,    0,    0, 2120,    0,    0,    0,    0,
    0,    0,    0, 2394,    0,    0,    0,    0,  489,  113,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 6983,    0,    0,    0,    0,    0,    0,    0,
  113,  113,    0,    0,    0, 7091,    0,    0,    0,    0,
    0, 7199, 7307, 7415,    0, 7523, 7631, 7739, 7847,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 7955,    0, 8063,
    0,    0,    0,    0, 8171, 8279, 8387, 8495, 8603,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0, 8711,    0, 8819,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  113,
    0,    0,    0,    0,    0,    0,    0,  113,    0,  113,
  113,  113,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,
  };
  protected static readonly short [] yyGindex = {            0,
    0, 1024,  955,    0,    0,    0,    0,  825,  817, 1041,
 1890,   -6,  696,    0,  360,  155, -178,    0,  736,  743,
 -309, -591,    0,  465,    0, -718,    0,  401,  792,  945,
   21,    0,  -12,    0,  806,    0,  -88,    0,  653, -115,
 -227,   -3,    0,  -70, 1004,  -63,    0, -208,    0,  794,
    0,    0, -159,    0,    0,    0,    0, -770, -116,    0,
 -594, -620,   96,  205,  206, -360, -268,    0,  665,  671,
  605, -396, 3030,    0,  165,    0,  485,    0,  290,    0,
  176,  -94,  177,    0,  393,    0,  623,  166,
  };
  protected static readonly short [] yyTable = {            68,
  129,  254,   70,  122,  730, 1001,  255,  100,  136,   68,
  303,   64,  453,  453,  460,  285,  735,  108,  195,  314,
  335,  427,  106,  124,  690,  823,   69,  824,  337,  825,
 1048,  470,  780,  826,  158,  290,   80,  124,  331, 1077,
  124,  162,   65, 1078,  565, 1079,  119,  331,  199,  441,
  574,  580,   17,  124,  647,  348,  115,  175,   39,  704,
  651,  284,  414,   68,   68,  659,   74,  714,  132,  774,
  776,  783,   68,  593,   63,   68,  138,  283,  139,  785,
  711,  100,  418,  100,  820,  100,  100,  421,  146,  156,
  125,  128,  133,  875,  885, 1069, 1075,   63,   20,  135,
  139,  134,  138,  166,  167,  311,  312,   72,  291,  237,
   72,  296,  115,  171,  238,  916,  956,  179,  280,  330,
   68,  100,   68,  290,  434,  139,  189,  107,  463,   68,
  256,   21,  139,  257,  290,  599,  202,  973,  286,  833,
  228,  597,  437,  231,  232,  600,  234,  182,  601,  183,
  434,  575,  107,  874,  124,  876,  198,  346,  880,  528,
  171,  434,  695,  529,  258,  259,  434,  261,  263,   18,
   19,  264,  287,  705,  594,  479,   39,  420,   86,   87,
  118,  712, 1008,  192, 1010,   22,  100,   58,   59,  571,
   72,  287,   76,  436,  170,  480,  300,  301,  916,  438,
   78, 1021,  192,  419,   52,   53,  605,   83,  608,  565,
  572,  192,   74,  131,  239,   23,   84,  738,  739,  132,
   88,   89,  315,  316,   24,  127,  319,  138,  321,  107,
  568,  124,  569,   25,   26,   27,   28,   29, 1042,  288,
   30,  260,  433,  133,  163,  925,  604,  586,  145,  107,
  149,  139,  134,   85,  946,  137,  107,  179,  347,  107,
  290,  521,  157,  104,  109,  495,  105,  498,  329,  197,
   93,  110,  200,  111,  507,  189,  139,  329,  100,  112,
  107,  113,  161,  100,  484,  292,  470,  281,  453,  325,
  282,  485,  326,  470,  163,  107,  428,  124,  103,  105,
  807,  808,  120,  435,  811,   58,   59,  124,  130,  133,
 1083, 1084,  107,  131,   64,  451,  451,  107,  462,  107,
  978,  326,  602,   86,   87,  603,  832,   88,   89,  603,
  426,  995,  996,  132,  469,  768,   86,   87,  561, 1047,
   88,   89,  107,  843,  107,   65,  140,  923,   68,  107,
  924,  489,  302,  494,   92,  497,  141,  500,  502,  107,
  504,  505,  506,  509,  511,  512,  513,  514,  107,  142,
  107,  155,  107,  520,  523,  483,  107,   63,  532,   97,
  105,  124,  107,  160, 1024,  164,  107,  706,  107,  107,
 1026,  107,  522,  557,  107,   68,  165,  563,  560, 1031,
  107,  222, 1034, 1000,  168,  107,  100,  107,  721,  107,
  107,  107,  898,  899,  616,   93,  902,  903,  172,  107,
  579,  107,  559,  124,  107,  106,  975,  733,   93,  976,
  174,  176,  223,  107,  107,  107,  107, 1052, 1053,  107,
  128,  124, 1022,  128,  184,  976,  107,  107,  107,  185,
  107,  107,  107,  144,  107,  148,  201,  152,  154, 1064,
 1045,  107,  107,  976,  221,  205,  206,  207,  107,  208,
  209,  210, 1046,  211,  203,  603,  107,   68,   68, 1085,
  139, 1086,  976,  618,  976,  236,  620,  214,  622,  623,
  119,  625,  626,  193,  628,  215,  630,  242,  703,  172,
  634,  635,  172,  637,  614,  615,  106,  970,   67,  243,
  252,   67,  163,  163,  648,  650,  163,  163,  244,  163,
  253,  654,  655,  656,  658,  262,  496,   24,  499,  301,
  817,  503,  301,  297,  163,  163,   25,   26,   27,   28,
   29,  192,  304,   30,  827,  828,  305,  124,  672,  306,
  307,  693,  313,   68,   68,   68,  322,  699,  702,  323,
  768,  768,  324,  251,  556,  333,  558,  334,  338,  107,
  163,  713,  341,  336,  716,  717,  718,  719,  340,  415,
  697,  698,  701,  422,  107,  107,  107,  107,  417,  469,
  423,  451,  107,  439,  440,  442,  469,  203,  205,  206,
  207,  443,  208,  209,  210,  445,  211,  467,  473,   68,
  990,  474,  139,  212,  213,  476,  107,  475,  477,  492,
  214,  308,  308,  308,  526,  562,    1,    2,  215,  287,
    3,    4,  815,    5,  573,  772,  740,  107,  576,  124,
  906,  581,  775,  251,  777,  577,  777,  578,    6,    7,
  784,  582,  914,  787,  788,  789,  790,  791,  792,  793,
  794,  795,  796,  797,  798,  583,  619,  587,  584,  588,
  124,  619,  470,  589,  619,  107,  170,  124,  590,  170,
  591,  592,  800,  619,    8,  595,  596,  416,  598,  894,
  895,  609,   68,   68,  611,   68,   68,  642,  643,  644,
  645,  646,  653,  660,  429,  430,  431,  432,  661,  708,
  308,  662,  663,  664,  665,  666,  709,  667,  668,  809,
  810,  216,  813,  814,  669,  670,  671,  710,  720,  837,
  722,  466,  723,  724,  290,  290,  217,  218,  219,  220,
  726,  725,  203,  203,  727,  728,  203,  203,  203,  203,
  734,  479,  478,  736,  482,  741,  486,  818,  744,  747,
  750,  752,  754,  755,  203,  203,  922,  777,  694,  777,
  203,  203,  777,  756,  759,  515,  516,  517,  518,  519,
  962,  963,  525,  761,  966,  967,  533,  534,  535,  536,
  537,  538,  539,  540,  541,  542,  543,  544,  762,  203,
  203,  763,  290,  290,  764,  779,  290,  290,   68,  802,
  804,  139,  819,  822,  830,  124,  124,  834,  840,  124,
  124,  124,  124,  829,  831,  845,  846,  451,  920,  848,
  849,  851,  852,  854,  585,  904,  856,  124,  124,  860,
  861,  224,  863,  124,  124,  868,  869,  870, 1015, 1016,
  871,  872, 1017, 1018,  873,  290,  290,  290,  290,  879,
   64,  926,  881,  882,  124,   20,  883,  887,  777,  888,
  426,  426,  124,  124,  890,  951,  952,   64,  892,   25,
  893,  690,  905,  908,  290,  915,  913,  945,  944,  165,
  927,   65,  947,  948,  949,  950,  953,  299,  955,  958,
  961,  971,  972, 1044,  977,  107,  979,  124,   65,  982,
  451,  994,  997, 1012,  999, 1002,  998,  837,  139,  107,
 1003,  124, 1004,   63,  652, 1005,  317,  318, 1009,  320,
  976, 1013, 1014, 1020,  426,  426,  426, 1028,  426,  426,
   63, 1029, 1030,  426,  332,  426, 1032, 1027,  426,  426,
  426,  426,  426,  426,  426,  426,  426,  426, 1033,  426,
  426, 1035,  426,  426,  426,  426,  426,  426,  426,  426,
  426,  426,  426,  426,  426,  469,  426,  426, 1036, 1037,
  715, 1038,  426,  426,  426,  426,  426, 1055,  426,  426,
  426,  426,  426,  426,  426,  425,  426, 1039,   22, 1040,
 1054, 1041, 1056, 1057, 1058, 1059, 1061,  426,   24, 1062,
 1065, 1063, 1066, 1067,  444, 1073,  446, 1081, 1082,  426,
  426,  426,  426,  205,  206,  207,  426,  208,  209,  210,
 1074,  211,    1,   77,  143,  144,  145,   46,  107,  124,
  124, 1060,  765,  766,  767,  214,  771,  773,  107,  146,
  124,  147, 1068,  215, 1070, 1071, 1072,  159,  327,  786,
   94,  461,  731,  173,   23,  459,  570,  328,  343,  339,
  121, 1023,  566,   24,  942,  345,  943,  107,  567,  124,
  606,   23,   25,   26,   27,   28,   29,  732,  993,   30,
   24,  891,  981,  812,  126,    0,   19,    0,  607,   25,
   26,   27,   28,   29,    0,  816,   30,   39,   40,   41,
    0,   42,   43,    0,  821,   45,    0,    0,   46,   47,
   48,   49,   50,    0,   51,    0,    0,    0,    0,    0,
  332,    0,    0,   20,   20,  158,    0,   20,   20,    0,
   20,    0,    0,    0,    0,    0,    0,   25,   25,  276,
    0,   25,   25,    0,   25,   20,   20,  165,  165,    0,
    0,  165,  165,    0,  165,    0,  487,  488,    0,   25,
   25,    0,    0,  124,    0,  610,    0,    0,    0,  165,
  165,   54,    0,  884,  617,  886,    0,    0,    0,  621,
    0,   20,  624,  222,    0,  627,    0,  629,    0,  631,
  632,  633,    0,    0,  636,   25,  638,  639,  640,  641,
    0,    0,    0,    0,    0,  165,    0,    0,    0,    0,
  907,  700,    0,    0,  223,    0,    0,    0,    0,    0,
    0,    0,    0,   55,   56,   57,   58,   59,   60,   61,
   62,   86,   87,    0,    0,   88,   89,   90,   32,   91,
   33,   34,   35,   36,   37,   38,  221,    0,    0,    0,
    0,    0,   44,   64,    0,    0,   22,   22,  940,  941,
   22,   22,   92,   22,    0,    0,   24,   24,    0,    0,
   24,   24,    0,   24,  265,  954,    0,    0,   22,   22,
    0,    0,  960,    0,   65,    0,   36,    0,   24,   24,
  266,    0,    0,    0,    0,   46,   46,    0,    0,  974,
    0,    0,   46,  742,    0,  743,    0,  745,  746,    0,
  748,  749,    0,  751,   22,  753,   63,   46,   46,  757,
  758,    0,  760,   93,   24,    0,    0,    0,    0,    0,
  124,    0,    0,    0,  267,  268,  269,    0,    0,  781,
  782,  270,  271,    0,  272,  273,  274,  275,    0,    0,
    0,    0,    0,   46,   19,   19,    0,  799,   19,   19,
    0,   19,    0,    0,    0,    0,    0,    0,    0,    0,
  124,  124,  124,    0,  124,  801,   19,   19,  803,    0,
  205,  206,  207,    0,  208,  209,  210,    0,  211,    0,
  124,    0,  124,  158,  158,  212,  213,  158,  158,    0,
  158,    0,  214, 1043,    0,    0,    0,    0,    0,    0,
  215,    0,   19,    0,    0,  158,  158,    0, 1049, 1050,
 1051,  124,    0,  124,    0,    0,  844,    0,    0,  847,
    0,    0,  850,    0,    0,  853,    0,  855,    0,  857,
  858,  859,  124,    0,  862,    0,  864,  865,  866,  867,
    0,  158,    0,  124,    0,  124,    0,   23,    0, 1076,
    0,    0,  877,    0,    0, 1080,   24,    0,    0,    0,
    0,    0,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,  889,    0,   31,    0,    0,
    0,    0,   32,    0,   33,   34,   35,   36,   37,   38,
   39,   40,   41,  216,   42,   43,   44,    0,   45,    0,
    0,   46,   47,   48,   49,   50,    0,   51,  217,  218,
  219,  220,  921,    0,    0,    0,  164,    0,   52,   53,
  929,  930,    0,  931,  932,    0,  933,  934,    0,  935,
  342,  936,    0,    0,    0,  937,  938,    0,  939,    0,
    0,    0,    0,    0,   36,   36,    0,    0,   36,   36,
    0,   36,    0,    0,    0,  192,    0,    0,  192,    0,
    0,    0,    0,  957,   54,  959,   36,   36,    0,    0,
    0,    0,    0,    0,  192,    0,    0,  124,  124,  124,
  123,  124,  124,  124,    0,  124,    0,    0,  124,  124,
   34,    0,  124,  124,  124,  124,  124,  992,    0,  124,
   43,    0,   36,    0,    0,  192,    0,  124,    0,    0,
  124,  124,    0,    0,  124,    0,   55,   56,   57,   58,
   59,   60,   61,   62,    0,    0, 1006, 1007,  124,  124,
    0,  124,  124, 1011,    0,  124,  124,  192,  124,  124,
  124,  124,  124,    0,  124,   29,  124,    0,    0,    0,
    0,    0,    0,    0,    0,   38,    0,  124,  124,  124,
    0,  124,  124,    0,    0,  265,  124,    0,  124,    0,
    0,  124,  124,  124,  124,  124,  124,  124,  124,  124,
  124,  266,  124,  124,    0,  124,  124,  124,  124,  124,
  124,  124,  124,  124,  124,  124,  124,  124,    0,  124,
  124,  124,    0,  123,    0,  124,  124,  124,  124,  124,
    0,  124,  124,  124,  124,  124,  124,  124,  124,  124,
    0,  117,    0,    0,    0,  267,  268,  269,    0,    0,
  124,    0,  270,  271,    0,  272,  273,  274,  275,    0,
    0,    0,  124,  124,  124,  124,    0,  124,    0,  124,
  124,    0,    0,  124,  124,  124,    0,    0,  124,  124,
  124,  117,  117,  117,    0,  117,    0,    0,    0,    0,
    0,  192,  192,  192,    0,  192,  192,  192,  192,  192,
    0,  117,    0,  117,  164,  164,  192,  192,  164,  164,
    0,  164,    0,  192,    0,    0,  192,  192,  192,  192,
  192,  192,    0,  192,    0,    0,  164,  164,  192,    0,
    0,    0,  117,    0,  117,    0,    0,    0,    0,    0,
    0,    0,  192,  192,    0,  192,  192,    0,    0,  192,
    0,    0,  192,  192,  192,  192,  192,    0,  192,    0,
    0,    0,  164,    0,  117,    0,  117,    0,  123,  123,
    0,    0,  123,  123,  123,  123,    0,    0,   34,   34,
    0,    0,   34,   34,    0,   34,    0,   44,   43,   43,
  123,  123,   43,   43,    0,   43,  123,  123,    0,    0,
   34,   34,    0,    0,    0,    0,    0,    0,    0,    0,
   43,   43,    0,    0,  192,  192,    0,  125,    0,    0,
    0,    0,    0,    0,    0,  123,  123,    0,    0,  192,
  192,  192,  192,   29,   29,    0,   34,   29,   29,    0,
   29,    0,    0,   38,   38,    0,   43,   38,   38,    0,
   38,    0,    0,    0,    0,   29,   29,    0,   27,    0,
    0,    0,    0,    0,    0,   38,   38,  192,  192,  192,
    0,    0,  192,  192,  192,    0,  195,    0,    0,  195,
    0,    0,    0,  143,    0,  147,  150,  151,  153,    0,
    0,   29,    0,    0,    0,  195,    0,    0,  117,  117,
  117,   38,  117,  117,  117,    0,  117,    0,   41,  117,
  117,    0,    0,  117,  117,  117,  117,  117,    0,    0,
  117,    0,    0,    0,    0,    0,  195,    0,  117,    0,
    0,  117,  117,  203,  204,  117,   30,  229,  230,    0,
    0,  233,    0,  235,    0,   32,    0,    0,    0,  117,
  117,    0,  117,  117,    0,    0,  117,  117,  195,  117,
  117,  117,  117,  117,    0,  117,    0,  117,    0,   33,
    0,    0,    0,    0,    0,    0,    0,    0,  117,  117,
  117,    0,  117,  117,    0,    0,    0,  117,    0,  117,
    0,    0,  117,  117,  117,  117,  117,  117,  117,  117,
  117,  117,    0,  117,  117,    0,  117,  117,  117,  117,
  117,  117,  117,  117,  117,  117,  117,  117,  117,   42,
  117,  117,    0,    0,    0,  117,  117,  117,  117,  117,
  117,    0,  117,  117,  117,  117,  117,  117,  117,  117,
  117,    0,    0,    0,    0,    0,    0,    0,    0,    0,
   97,  117,    0,    0,    0,   44,   44,    0,    0,   44,
   44,    0,   44,  117,  117,  117,  117,    0,  117,    0,
  117,  117,    0,    0,  117,  117,  117,   44,   44,  117,
  117,  117,  108,  266,    0,    0,  266,    0,    0,    0,
    0,    0,  195,  195,  195,    0,  195,  195,  195,  195,
  195,   64,    0,    0,  266,    0,    0,  195,  195,    0,
    0,    0,    0,   44,  195,    0,    0,  195,  195,  195,
  195,  195,  195,  298,  195,    0,   27,   27,    0,  195,
   27,   27,   65,   27,    0,  266,    0,    0,    0,  222,
    0,    0,    0,  195,  195,    0,  195,  195,   27,   27,
  195,    0,    0,  195,  195,  195,  195,  195,    0,  195,
    0,    0,    0,    0,   63,  266,  114,  266,   98,    0,
  223,    0,    0,    0,    0,    0,   41,   41,    0,    0,
   41,   41,    0,   41,   27,   97,    0,    0,    0,    0,
  107,    0,  124,    0,    0,    0,    0,    0,   41,   41,
    0,   97,  221,    0,   30,   30,    0,    0,   30,   30,
    0,   30,    0,   32,   32,  195,  195,   32,   32,    0,
   32,    0,    0,    0,    0,    0,   30,   30,    0,    0,
  195,  195,  195,  195,   41,   32,   32,   33,   33,    0,
    0,   33,   33,    0,   33,   97,   97,   97,    0,    0,
    0,    0,   97,   97,    0,   97,   97,   97,   97,   33,
   33,    0,   30,    0,    0,    0,    0,    0,  195,  195,
  195,   32,    0,  195,  195,  195,  545,  546,  547,  548,
  549,  550,  551,  552,  553,  554,    0,   42,   42,    0,
    0,   42,   42,   35,   42,   33,    0,    0,  107,  266,
  124,    0,    0,   98,    0,   23,    0,    0,    0,   42,
   42,    0,    0,    0,   24,  266,  266,    0,  266,   98,
    0,    0,    0,   25,   26,   27,   28,   29,    0,    0,
   30,    0,    0,    0,    0,    0,  205,  206,  207,    0,
  208,  209,  210,    0,  211,   42,    0,    0,    0,    0,
    0,  212,  213,    0,    0,    0,    0,    0,  214,    0,
    0,    0,    0,   98,   98,   98,  215,    0,    0,    0,
   98,   98,    0,   98,   98,   98,   98,    0,    0,  266,
  266,  266,    0,  266,  266,    0,    0,    0,  266,    0,
  266,    0,    0,  266,  266,  266,  266,  266,  266,  266,
  266,  266,  266,    0,  266,  266,    0,  266,  266,  266,
  266,  266,  266,  266,  266,  266,  266,  266,  266,  266,
    0,  266,  266,  431,  431,    0,    0,  266,  266,  266,
  266,  266,  266,  266,  266,  266,  266,  266,  266,  266,
  107,  266,  124,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  266,    0,    0,    0,    0,    0,    0,  216,
    0,    0,    0,  107,  266,  266,  266,  266,    0,    0,
    0,  266,    0,    0,  217,  218,  219,  220,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  431,  431,  431,
    0,  431,  431,    0,    0,    0,  431,    0,  431,    0,
    0,  431,  431,  431,  431,  431,  431,  431,  431,  431,
  431,    0,  431,  431,    0,  431,  431,  431,  431,  431,
  431,  431,  431,  431,  431,  431,  431,  431,    0,  431,
  431,  415,  415,    0,    0,  431,  431,  431,  431,  431,
    0,  431,  431,  431,  431,  431,  431,  431,  107,  431,
  124,  673,  674,    0,    0,    0,    0,    0,    0,    0,
  431,   35,   35,    0,    0,   35,   35,    0,   35,    0,
  348,  348,  431,  431,  431,  431,    0,    0,    0,  431,
    0,    0,    0,   35,   35,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  415,  415,  415,    0,  415,
  415,    0,    0,    0,  415,    0,  415,    0,   64,  415,
  415,  415,  415,  415,  415,  415,  415,  415,  415,   35,
  415,  415,    0,  415,  415,  415,  415,  415,  415,  415,
  415,  415,  415,  415,  415,  415,    0,  415,  415,   65,
    0,    0,    0,  415,  415,  415,  415,  415,    0,  415,
  415,  415,  415,  415,  415,  415,  107,  415,  124,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  415,    0,
    0,   63,    0,  388,  388,    0,    0,    0,    0,    0,
  415,  415,  415,  415,    0,    0,    0,  415,    0,    0,
    0,    0,  675,  676,  677,  678,    0,    0,    0,    0,
    0,  679,  680,  681,  682,  683,  684,  685,  686,  687,
  688,  348,  348,  348,  348,    0,    0,    0,    0,    0,
  348,  348,  348,  348,  348,  348,  348,  348,  348,  348,
    0,    0,    0,    0,    0,    0,    0,  388,  388,  388,
    0,  388,  388,    0,    0,    0,  388,    0,  388,    0,
    0,  388,  388,  388,  388,  388,  388,  388,  388,  388,
  388,    0,  388,  388,    0,  388,  388,  388,  388,  388,
  388,  388,  388,  388,  388,  388,  388,  388,    0,  388,
  388,  385,  385,    0,    0,  388,  388,  388,  388,  388,
    0,  388,  388,  388,  388,  388,  388,  388,  107,  388,
  124,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  388,    0,   23,    0,    0,    0,    0,    0,    0,    0,
    0,   24,  388,  388,  388,  388,    0,    0,    0,  388,
   25,   26,   27,   28,   29,    0,    0,   30,    0,    0,
    0,    0,    0,    0,    0,  385,  385,  385,    0,  385,
  385,    0,    0,    0,  385,    0,  385,    0,    0,  385,
  385,  385,  385,  385,  385,  385,  385,  385,  385,    0,
  385,  385,    0,  385,  385,  385,  385,  385,  385,  385,
  385,  385,  385,  385,  385,  385,    0,  385,  385,  386,
  386,    0,    0,  385,  385,  385,  385,  385,    0,  385,
  385,  385,  385,  385,  385,  385,  107,  385,  124,    0,
    0,    0,  501,    0,    0,    0,    0,    0,  385,  205,
  206,  207,    0,  208,  209,  210,    0,  211,    0,    0,
  385,  385,  385,  385,  983,  984,    0,  385,  985,    0,
    0,  214,    0,    0,    0,    0,    0,    0,    0,  215,
    0,    0,    0,  386,  386,  386,    0,  386,  386,    0,
    0,    0,  386,    0,  386,    0,   64,  386,  386,  386,
  386,  386,  386,  386,  386,  386,  386,    0,  386,  386,
    0,  386,  386,  386,  386,  386,  386,  386,  386,  386,
  386,  386,  386,  386,    0,  386,  386,   65,    0,    0,
    0,  386,  386,  386,  386,  386,    0,  386,  386,  386,
  386,  386,  386,  386,  107,  386,  124,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  386,    0,    0,   63,
    0,  387,  387,    0,    0,    0,    0,    0,  386,  386,
  386,  386,  986,    0,    0,  386,  107,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  987,  988,  989,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  387,  387,  387,    0,  387,
  387,    0,    0,    0,  387,    0,  387,    0,    0,  387,
  387,  387,  387,  387,  387,  387,  387,  387,  387,    0,
  387,  387,    0,  387,  387,  387,  387,  387,  387,  387,
  387,  387,  387,  387,  387,  387,    0,  387,  387,  428,
  428,    0,    0,  387,  387,  387,  387,  387,    0,  387,
  387,  387,  387,  387,  387,  387,  107,  387,  124,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  387,    0,
   23,    0,    0,    0,    0,    0,    0,    0,    0,   24,
  387,  387,  387,  387,    0,    0,    0,  387,   25,   26,
   27,   28,   29,    0,    0,   30,    0,    0,    0,    0,
    0,    0,    0,  428,  428,  428,    0,  428,  428,    0,
    0,    0,  428,    0,  428,    0,    0,  428,  428,  428,
  428,  428,  428,  428,  428,  428,  428,    0,  428,  428,
    0,  428,  428,  428,  428,  428,  428,  428,  428,  428,
  428,  428,  428,  428,    0,  428,  428,  419,  419,    0,
    0,  428,  428,  428,  428,  428,    0,  428,  428,  428,
  428,  428,  428,  428,  107,  428,  124,    0,    0,    0,
  508,    0,    0,    0,    0,    0,  428,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  428,  428,
  428,  428,    0,    0,    0,  428,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  419,  419,  419,    0,  419,  419,    0,    0,    0,
  419,    0,  419,    0,   64,  419,  419,  419,  419,  419,
  419,  419,  419,  419,  419,    0,  419,  419,    0,  419,
  419,  419,  419,  419,  419,  419,  419,  419,  419,  419,
  419,  419,    0,  419,  419,   65,    0,    0,    0,  419,
  419,  419,  419,  419,    0,  419,  419,  419,  419,  419,
  419,  419,  107,  419,  124,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  419,    0,    0,   63,    0,  411,
  411,    0,    0,    0,    0,    0,  419,  419,  419,  419,
    0,    0,    0,  419,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  411,  411,  411,    0,  411,  411,    0,
    0,    0,  411,    0,  411,    0,    0,  411,  411,  411,
  411,  411,  411,  411,  411,  411,  411,    0,  411,  411,
    0,  411,  411,  411,  411,  411,  411,  411,  411,  411,
  411,  411,  411,  411,    0,  411,  411,  397,  397,    0,
    0,  411,  411,  411,  411,  411,    0,  411,  411,  411,
  411,  411,  411,  411,    0,  411,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  411,    0,   23,    0,
    0,    0,    0,    0,    0,    0,    0,   24,  411,  411,
  411,  411,    0,    0,    0,  411,   25,   26,   27,   28,
   29,    0,    0,   30,    0,    0,    0,    0,    0,    0,
    0,  397,  397,  397,    0,  397,  397,    0,    0,    0,
  397,    0,  397,    0,    0,  397,  397,  397,  397,  397,
  397,  397,  397,  397,  397,    0,  397,  397,    0,  397,
  397,  397,  397,  397,  397,  397,  397,  397,  397,  397,
  397,  397,  222,  397,  397,  358,  358,    0,    0,  397,
  397,  397,  397,  397,    0,  397,  397,  397,  397,  397,
  397,  397,    0,  397,  805,  806,    0,    0,  510,    0,
    0,    0,    0,  223,  397,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  397,  397,  397,  397,
    0,    0,    0,  397,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  221,    0,  841,  842,  358,
  358,  358,    0,  358,  358,    0,    0,    0,  358,    0,
  358,    0,    0,  358,  358,  358,  358,  358,  358,  358,
  358,  358,  358,    0,  358,  358,    0,  358,  358,  358,
  358,  358,  358,  358,  358,  358,  358,  358,  358,  358,
    0,  358,  358,    0,    0,    0,    0,  358,  358,  358,
  358,  358,    0,  358,  358,  358,  358,  358,  358,  358,
    0,  358,    0,    0,    0,    0,  896,  897,    0,    0,
  900,  901,  358,    0,    0,  222,    0,    0,    0,    0,
    0,    0,    0,    0,  358,  358,  358,  358,    0,    0,
    0,  358,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  928,    0,    0,    0,  223,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   64,    0,    0,    0,  205,
  206,  207,    0,  208,  209,  210,    0,  211,  221,   64,
    0,    0,    0,    0,  212,  213,    0,    0,    0,    0,
    0,  214,    0,    0,    0,    0,   65,  964,  965,  215,
    0,  968,  969,    0,    0,    0,  126,    0,    0,    0,
   65,    0,    0,    0,    0,   64,    0,    0,    0,    0,
   40,   41,    0,   42,   43,    0,    0,   45,   63,    0,
   46,   47,   48,   49,   50,    0,   51,    0,    0,    0,
    0,    0,   63,    0,    0,    0,   65,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  151,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 1019,
    0,    0,    0,    0,    0,    0,    0,    0,   63,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  151,
    0,    0,  216,   54,    0,    0,  107,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  217,  218,  219,
  220,    0,  205,  206,  207,    0,  208,  209,  210,    0,
  211,  151,    0,    0,    0,    0,    0,  212,  213,    0,
    0,    0,    0,    0,  214,    0,    0,    0,    0,  424,
    0,    0,  215,    0,    0,   55,   56,   57,    0,  126,
   60,   61,   62,    0,    0,  222,    0,    0,    0,    0,
    0,    0,    0,   40,   41,    0,   42,   43,    0,   23,
   45,    0,    0,   46,   47,   48,   49,   50,   24,   51,
    0,    0,    0,  244,    0,    0,  223,   25,   26,   27,
   28,   29,   24,    0,   30,    0,    0,    0,    0,  126,
    0,   25,   26,   27,   28,   29,    0,    0,   30,    0,
    0,    0,   39,   40,   41,    0,   42,   43,  221,   23,
   45,    0,    0,   46,   47,   48,   49,   50,   24,   51,
    0,    0,    0,    0,    0,  216,   54,   25,   26,   27,
   28,   29,    0,    0,   30,    0,    0,    0,    0,  126,
  217,  218,  219,  220,    0,    0,    0,    0,    0,    0,
  152,    0,  151,   40,   41,    0,   42,   43,    0,    0,
   45,  151,    0,   46,   47,   48,   49,   50,   92,   51,
  151,  151,  151,  151,  151,    0,   54,  151,   55,   56,
   57,  152,  151,   60,   61,   62,    0,    0,    0,    0,
    0,    0,  909,   64,    0,    0,  151,  151,    0,  151,
  151,    0,    0,  151,    0,    0,  151,  151,  151,  151,
  151,  151,  151,  152,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   65,    0,   54,    0,   55,   56,
   57,   58,   59,   60,   61,   62,   64,    0,    0,    0,
    0,    0,  205,  206,  207,    0,  208,  209,  210,    0,
  211,    0,    0,    0,    0,    0,   63,  212,  213,    0,
    0,    0,    0,    0,  214,    0,    0,   65,    0,  151,
    0,    0,  215,    0,    0,    0,    0,    0,   55,   56,
   57,    0,    0,   60,   61,   62,  156,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   63,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  156,    0,    0,
    0,  151,  151,  151,    0,    0,  151,  151,  151,  157,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  156,
    0,    0,    0,    0,  152,    0,    0,    0,    0,    0,
  157,    0,    0,  152,    0,  216,    0,    0,    0,  107,
    0,    0,  152,  152,  152,  152,  152,    0,    0,  152,
  217,  218,  219,  220,  152,    0,    0,    0,    0,    0,
    0,    0,  157,    0,    0,    0,    0,   23,  152,  152,
  222,  152,  152,    0,    0,  152,   24,    0,  152,  152,
  152,  152,  152,  152,  152,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,    0,   79,    0,    0,
    0,  223,    0,    0,    0,    0,    0,    0,    0,    0,
   23,   40,   41,  222,   42,   43,    0,    0,   45,   24,
   64,   46,   47,   48,   49,   50,    0,   51,   25,   26,
   27,   28,   29,  221,    0,   30,    0,    0,    0,    0,
  126,  152,    0,    0,  223,    0,    0,    0,    0,    0,
    0,   65,    0,    0,   40,   41,    0,   42,   43,    0,
  156,   45,    0,    0,   46,   47,   48,   49,   50,  156,
   51,    0,    0,    0,    0,    0,  221,    0,  156,  156,
  156,  156,  156,   63,   54,  156,    0,    0,    0,    0,
  156,    0,    0,  152,  152,  152,    0,    0,  152,  152,
  152,    0,    0,  157,  156,  156,    0,  156,  156,    0,
    0,  156,  157,    0,  156,  156,  156,  156,  156,    0,
  156,  157,  157,  157,  157,  157,    0,   54,  157,    0,
    0,    0,    0,  157,    0,    0,   55,   56,   57,    0,
    0,   60,   61,   62,  564,    0,    0,  157,  157,    0,
  157,  157,    0,    0,  157,    0,    0,  157,  157,  157,
  157,  157,    0,  157,    0,    0,    0,  205,  206,  207,
    0,  208,  209,  210,    0,  211,    0,  156,    0,   55,
   56,   57,  212,  213,   60,   61,   62,    0,    0,  214,
    0,    0,    0,    0,    0,    0,    0,  215,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  205,  206,  207,    0,  208,  209,  210,    0,  211,    0,
  157,    0,    0,    0,   23,  212,  213,    0,    0,  156,
  156,  156,  214,   24,  156,  156,  156,    0,    0,    0,
  215,    0,   25,   26,   27,   28,   29,    0,    0,   30,
   64,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  707,
    0,    0,  157,  157,  157,    0,  349,  157,  157,  157,
    0,   65,    0,    0,    0,    0,    0,    0,    0,    0,
  216,    0,  350,    0,  107,  351,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  217,  218,  219,  220,  530,
    0,    0,    0,   63,    0,  247,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  216,    0,    0,    0,    0,    0,    0,
    0,    0,  531,    0,    0,    0,    0,    0,  217,  218,
  219,  220,    0,    0,    0,    0,  352,  353,  354,    0,
  355,  356,    0,    0,    0,  357,    0,  358,    0,    0,
  359,  360,  361,  362,  363,  364,  365,  366,  367,  368,
    0,  369,  370,    0,  371,  372,  373,  374,  375,  376,
  377,  378,  379,  380,  381,  382,  383,    0,  384,  385,
    0,  349,  285,    0,  386,  387,  388,  389,  390,    0,
  391,  392,  393,  394,  395,  396,  397,  350,  398,    0,
  351,    0,    0,    0,    0,    0,    0,    0,    0,  399,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  400,  401,  402,  403,    0,    0,    0,  404,    0,
    0,    0,    0,    0,  244,  245,    0,    0,    0,    0,
    0,    0,    0,   24,  246,    0,    0,    0,    0,    0,
    0,    0,   25,   26,   27,   28,   29,    0,    0,   30,
    0,  352,  353,  354,    0,  355,  356,    0,    0,    0,
  357,    0,  358,    0,   64,  359,  360,  361,  362,  363,
  364,  365,  366,  367,  368,    0,  369,  370,    0,  371,
  372,  373,  374,  375,  376,  377,  378,  379,  380,  381,
  382,  383,    0,  384,  385,   65,    0,  283,    0,  386,
  387,  388,  389,  390,  285,  391,  392,  393,  394,  395,
  396,  397,    0,  398,    0,    0,    0,    0,    0,    0,
  285,    0,    0,  285,  399,    0,    0,   63,    0,    0,
    0,    0,    0,    0,    0,    0,  400,  401,  402,  403,
    0,    0,    0,  404,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  285,  285,  285,    0,  285,  285,
    0,    0,    0,  285,    0,  285,    0,    0,  285,  285,
  285,  285,  285,  285,  285,  285,  285,  285,    0,  285,
  285,    0,  285,  285,  285,  285,  285,  285,  285,  285,
  285,  285,  285,  285,  285,    0,  285,  285,    0,  283,
  286,    0,  285,  285,  285,  285,  285,    0,  285,  285,
  285,  285,  285,  285,  285,  283,  285,    0,  283,    0,
    0,    0,    0,    0,    0,    0,    0,  285,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   23,  285,
  285,  285,  285,    0,    0,    0,  285,   24,    0,    0,
    0,    0,    0,    0,    0,    0,   25,   26,   27,   28,
   29,    0,    0,   30,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  283,
  283,  283,    0,  283,  283,    0,    0,    0,  283,    0,
  283,    0,    0,  283,  283,  283,  283,  283,  283,  283,
  283,  283,  283,    0,  283,  283,    0,  283,  283,  283,
  283,  283,  283,  283,  283,  283,  283,  283,  283,  283,
    0,  283,  283,  657,    0,  284,    0,  283,  283,  283,
  283,  283,  286,  283,  283,  283,  283,  283,  283,  283,
    0,  283,    0,    0,    0,    0,    0,    0,  286,    0,
    0,  286,  283,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  283,  283,  283,  283,    0,    0,
    0,  283,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  286,  286,  286,    0,  286,  286,    0,    0,
    0,  286,    0,  286,    0,    0,  286,  286,  286,  286,
  286,  286,  286,  286,  286,  286,    0,  286,  286,  188,
  286,  286,  286,  286,  286,  286,  286,  286,  286,  286,
  286,  286,  286,    0,  286,  286,    0,  284,   64,    0,
  286,  286,  286,  286,  286,  178,  286,  286,  286,  286,
  286,  286,  286,  284,  286,    0,  284,    0,    0,    0,
    0,    0,    0,    0,   64,  286,    0,    0,    0,   65,
    0,    0,    0,    0,    0,    0,    0,  286,  286,  286,
  286,   64,    0,    0,  286,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   65,    0,    0,   64,    0,
    0,   63,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   65,    0,    0,    0,    0,  284,  284,  284,
    0,  284,  284,    0,    0,    0,  284,   63,  284,   65,
    0,  284,  284,  284,  284,  284,  284,  284,  284,  284,
  284,    0,  284,  284,   63,  284,  284,  284,  284,  284,
  284,  284,  284,  284,  284,  284,  284,  284,    0,  284,
  284,   63,  729,    0,    0,  284,  284,  284,  284,  284,
    0,  284,  284,  284,  284,  284,  284,  284,    0,  284,
    0,   64,    0,    0,    0,    0,    0,    0,    0,    0,
  284,    0,    0,    0,    0,    0, 1025,    0,    0,    0,
    0,    0,  284,  284,  284,  284,    0,    0,    0,  284,
    0,    0,   65,    0,    0,   64,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  836,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   23,    0,   63,   64,   65,    0,    0,    0,
    0,   24,    0,    0,    0,    0,  186,    0,    0,    0,
   25,   26,   27,   28,   29,    0,    0,   30,   23,   64,
    0,    0,    0,  187,    0,    0,   65,   24,   63,    0,
    0,    0,    0,    0,   64,  244,   25,   26,   27,   28,
   29,    0,    0,   30,   24,    0,    0,    0,    0,  177,
   65,    0,   23,   25,   26,   27,   28,   29,   63,    0,
   30,   24,    0,    0,    0,   65,    0,    0,    0,    0,
   25,   26,   27,   28,   29,   64,    0,   30,    0,    0,
    0,  524,   63,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   63,  649,    0,
    0,    0,    0,    0,    0,    0,   65,    0,   64,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  205,  206,
  207,    0,  208,  209,  210,   23,  211,    0,   63,   65,
    0,   64,    0,    0,   24,    0,    0,  468,    0,   64,
  214,    0,    0,   25,   26,   27,   28,   29,  215,    0,
   30,    0,  205,  206,  207,    0,  208,  209,  210,   23,
  211,   63,   65,    0,    0,    0,   64,    0,   24,    0,
   65,  468,    0,    0,  214,    0,    0,   25,   26,   27,
   28,   29,  215,    0,   30,    0,    0,    0,    0,   23,
    0,    0,   64,    0,   63,    0,    0,   65,   24,    0,
    0,    0,   63,  835,  114,    0,    0,   25,   26,   27,
   28,   29,    0,   23,   30,    0,    0,    0,    0,    0,
   64,    0,   24,   65,    0,    0,    0,  186,   23,   63,
    0,   25,   26,   27,   28,   29,    0,   24,   30,    0,
    0,    0,    0,   64,  187,    0,   25,   26,   27,   28,
   29,   65,    0,   30,    0,   63,    0,    0,    0,  177,
    0,    0,  205,  206,  207,  447,  208,  209,  210,   23,
  448,    0,    0,    0,   65,    0,  980,   64,   24,  449,
    0,  450,    0,   63,  214,   64,    0,   25,   26,   27,
   28,   29,  215,    0,   30,  205,  206,  207,  447,  208,
  209,  210,   23,  448,    0,    0,   63,    0,   65,    0,
    0,   24,  458,    0,  450,   64,   65,  214,    0,    0,
   25,   26,   27,   28,   29,  215,    0,   30,  205,  206,
  207,    0,  208,  209,  210,   23,  211,    0,   64,    0,
   63,    0,    0,  244,   24,    0,   65,  468,   63,    0,
  214,    0,   24,   25,   26,   27,   28,   29,  215,    0,
   30,   25,   26,   27,   28,   29,    0,  348,   30,   65,
  244,  245,    0,    0,    0,    0,    0,    0,   63,   24,
  246,    0,    0,    0,    0,    0,    0,    0,   25,   26,
   27,   28,   29,    0,    0,   30,  244,  464,  348,    0,
    0,  310,    0,    0,    0,   24,  465,    0,    0,    0,
    0,    0,    0,    0,   25,   26,   27,   28,   29,    0,
    0,   30,    0,    0,   23,    0,    0,    0,    0,    0,
  348,    0,    0,   24,  919,    0,    0,    0,    0,    0,
    0,    0,   25,   26,   27,   28,   29,   23,    0,   30,
    0,    0,    0,    0,    0,    0,   24,    0,    0,    0,
    0,    0,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,    0,    0,    0,    0,
    0,   23,    0,    0,    0,    0,    0,    0,    0,   23,
   24,    0,    0,    0,    0,  835,    0,    0,   24,   25,
   26,   27,   28,   29,    0,    0,   30,   25,   26,   27,
   28,   29,    0,    0,   30,    0,    0,  349,    0,  244,
    0,    0,    0,    0,    0,    0,    0,    0,   24,    0,
    0,    0,    0,  350,    0,    0,  351,   25,   26,   27,
   28,   29,  244,    0,   30,    0,    0,    0,    0,    0,
    0,   24,    0,    0,    0,    0,    0,    0,    0,    0,
   25,   26,   27,   28,   29,    0,    0,   30,    0,    0,
    0,  348,    0,    0,    0,    0,    0,    0,    0,    0,
  348,    0,    0,    0,    0,    0,    0,    0,    0,  348,
  348,  348,  348,  348,    0,    0,  348,  352,  353,  354,
    0,  355,  356,    0,    0,    0,  357,    0,  358,    0,
    0,  359,  360,  361,  362,  363,  364,  365,  366,  367,
  368,    0,  369,  370,    0,  371,  372,  373,  374,  375,
  376,  377,  378,  379,  380,  381,  382,  383,    0,  384,
  385,    0,    0,    0,    0,  386,  387,  388,  389,  390,
    0,  391,  392,  393,  394,  395,  396,  397,  279,  398,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  399,    0,    0,    0,    0,    0,    0,  344,    0,  126,
    0,    0,  400,  401,  402,  403,    0,    0,    0,  404,
    0,    0,    0,   40,   41,    0,   42,   43,  126,    0,
   45,    0,    0,   46,   47,   48,   49,   50,    0,   51,
    0,    0,   40,   41,    0,   42,   43,    0,    0,   45,
    0,    0,   46,   47,   48,   49,   50,    0,   51,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,   54,    0,    0,  107,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  410,  410,    0,    0,   54,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   55,   56,
   57,    0,    0,   60,   61,   62,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   55,   56,   57,
    0,    0,   60,   61,   62,  410,  410,  410,    0,  410,
  410,    0,    0,    0,  410,    0,  410,    0,    0,  410,
  410,  410,  410,  410,  410,  410,  410,  410,  410,    0,
  410,  410,    0,  410,  410,  410,  410,  410,  410,  410,
  410,  410,  410,  410,  410,  410,    0,  410,  410,  379,
  379,    0,    0,  410,  410,  410,  410,  410,    0,  410,
  410,  410,  410,  410,  410,  410,    0,  410,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  410,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  410,  410,  410,  410,    0,    0,    0,  410,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  379,  379,  379,    0,  379,  379,    0,
    0,    0,  379,    0,  379,    0,    0,  379,  379,  379,
  379,  379,  379,  379,  379,  379,  379,    0,  379,  379,
    0,  379,  379,  379,  379,  379,  379,  379,  379,  379,
  379,  379,  379,  379,    0,  379,  379,  359,  359,    0,
    0,  379,  379,  379,  379,  379,    0,  379,  379,  379,
  379,  379,  379,  379,    0,  379,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  379,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  379,  379,
  379,  379,    0,    0,    0,  379,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  359,  359,  359,    0,  359,  359,    0,    0,    0,
  359,    0,  359,    0,    0,  359,  359,  359,  359,  359,
  359,  359,  359,  359,  359,    0,  359,  359,    0,  359,
  359,  359,  359,  359,  359,  359,  359,  359,  359,  359,
  359,  359,    0,  359,  359,  365,  365,    0,    0,  359,
  359,  359,  359,  359,    0,  359,  359,  359,  359,  359,
  359,  359,    0,  359,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  359,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  359,  359,  359,  359,
    0,    0,    0,  359,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  365,
  365,  365,    0,  365,  365,    0,    0,    0,  365,    0,
  365,    0,    0,  365,  365,  365,  365,  365,  365,  365,
  365,  365,  365,    0,  365,  365,    0,  365,  365,  365,
  365,  365,  365,  365,  365,  365,  365,  365,  365,  365,
    0,  365,  365,  360,  360,    0,    0,  365,  365,  365,
  365,  365,    0,  365,  365,  365,  365,  365,  365,  365,
    0,  365,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  365,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  365,  365,  365,  365,    0,    0,
    0,  365,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  360,  360,  360,
    0,  360,  360,    0,    0,    0,  360,    0,  360,    0,
    0,  360,  360,  360,  360,  360,  360,  360,  360,  360,
  360,    0,  360,  360,    0,  360,  360,  360,  360,  360,
  360,  360,  360,  360,  360,  360,  360,  360,    0,  360,
  360,  366,  366,    0,    0,  360,  360,  360,  360,  360,
    0,  360,  360,  360,  360,  360,  360,  360,    0,  360,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  360,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  360,  360,  360,  360,    0,    0,    0,  360,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  366,  366,  366,    0,  366,
  366,    0,    0,    0,  366,    0,  366,    0,    0,  366,
  366,  366,  366,  366,  366,  366,  366,  366,  366,    0,
  366,  366,    0,  366,  366,  366,  366,  366,  366,  366,
  366,  366,  366,  366,  366,  366,    0,  366,  366,  361,
  361,    0,    0,  366,  366,  366,  366,  366,    0,  366,
  366,  366,  366,  366,  366,  366,    0,  366,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  366,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  366,  366,  366,  366,    0,    0,    0,  366,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  361,  361,  361,    0,  361,  361,    0,
    0,    0,  361,    0,  361,    0,    0,  361,  361,  361,
  361,  361,  361,  361,  361,  361,  361,    0,  361,  361,
    0,  361,  361,  361,  361,  361,  361,  361,  361,  361,
  361,  361,  361,  361,    0,  361,  361,  372,  372,    0,
    0,  361,  361,  361,  361,  361,    0,  361,  361,  361,
  361,  361,  361,  361,    0,  361,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  361,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  361,  361,
  361,  361,    0,    0,    0,  361,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  372,  372,  372,    0,  372,  372,    0,    0,    0,
  372,    0,  372,    0,    0,  372,  372,  372,  372,  372,
  372,  372,  372,  372,  372,    0,  372,  372,    0,  372,
  372,  372,  372,  372,  372,  372,  372,  372,  372,  372,
  372,  372,    0,  372,  372,  396,  396,    0,    0,  372,
  372,  372,  372,  372,    0,  372,  372,  372,  372,  372,
  372,  372,    0,  372,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  372,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  372,  372,  372,  372,
    0,    0,    0,  372,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  396,
  396,  396,    0,  396,  396,    0,    0,    0,  396,    0,
  396,    0,    0,  396,  396,  396,  396,  396,  396,  396,
  396,  396,  396,    0,  396,  396,    0,  396,  396,  396,
  396,  396,  396,  396,  396,  396,  396,  396,  396,  396,
    0,  396,  396,  390,  390,    0,    0,  396,  396,  396,
  396,  396,    0,  396,  396,  396,  396,  396,  396,  396,
    0,  396,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  396,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  396,  396,  396,  396,    0,    0,
    0,  396,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  390,  390,  390,
    0,  390,  390,    0,    0,    0,  390,    0,  390,    0,
    0,  390,  390,  390,  390,  390,  390,  390,  390,  390,
  390,    0,  390,  390,    0,  390,  390,  390,  390,  390,
  390,  390,  390,  390,  390,  390,  390,  390,    0,  390,
  390,  367,  367,    0,    0,  390,  390,  390,  390,  390,
    0,  390,  390,  390,  390,  390,  390,  390,    0,  390,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  390,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  390,  390,  390,  390,    0,    0,    0,  390,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  367,  367,  367,    0,  367,
  367,    0,    0,    0,  367,    0,  367,    0,    0,  367,
  367,  367,  367,  367,  367,  367,  367,  367,  367,    0,
  367,  367,    0,  367,  367,  367,  367,  367,  367,  367,
  367,  367,  367,  367,  367,  367,    0,  367,  367,  362,
  362,    0,    0,  367,  367,  367,  367,  367,    0,  367,
  367,  367,  367,  367,  367,  367,    0,  367,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  367,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  367,  367,  367,  367,    0,    0,    0,  367,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  362,  362,  362,    0,  362,  362,    0,
    0,    0,  362,    0,  362,    0,    0,  362,  362,  362,
  362,  362,  362,  362,  362,  362,  362,    0,  362,  362,
    0,  362,  362,  362,  362,  362,  362,  362,  362,  362,
  362,  362,  362,  362,    0,  362,  362,  363,  363,    0,
    0,  362,  362,  362,  362,  362,    0,  362,  362,  362,
  362,  362,  362,  362,    0,  362,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  362,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  362,  362,
  362,  362,    0,    0,    0,  362,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  363,  363,  363,    0,  363,  363,    0,    0,    0,
  363,    0,  363,    0,    0,  363,  363,  363,  363,  363,
  363,  363,  363,  363,  363,    0,  363,  363,    0,  363,
  363,  363,  363,  363,  363,  363,  363,  363,  363,  363,
  363,  363,    0,  363,  363,  368,  368,    0,    0,  363,
  363,  363,  363,  363,    0,  363,  363,  363,  363,  363,
  363,  363,    0,  363,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  363,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  363,  363,  363,  363,
    0,    0,    0,  363,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  368,
  368,  368,    0,  368,  368,    0,    0,    0,  368,    0,
  368,    0,    0,  368,  368,  368,  368,  368,  368,  368,
  368,  368,  368,    0,  368,  368,    0,  368,  368,  368,
  368,  368,  368,  368,  368,  368,  368,  368,  368,  368,
    0,  368,  368,  377,  377,    0,    0,  368,  368,  368,
  368,  368,    0,  368,  368,  368,  368,  368,  368,  368,
    0,  368,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  368,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  368,  368,  368,  368,    0,    0,
    0,  368,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  377,  377,  377,
    0,  377,  377,    0,    0,    0,  377,    0,  377,    0,
    0,  377,  377,  377,  377,  377,  377,  377,  377,  377,
  377,    0,  377,  377,    0,  377,  377,  377,  377,  377,
  377,  377,  377,  377,  377,  377,  377,  377,    0,  377,
  377,  370,  370,    0,    0,  377,  377,  377,  377,  377,
    0,  377,  377,  377,  377,  377,  377,  377,    0,  377,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  377,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  377,  377,  377,  377,    0,    0,    0,  377,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  370,  370,  370,    0,  370,
  370,    0,    0,    0,  370,    0,  370,    0,    0,  370,
  370,  370,  370,  370,  370,  370,  370,  370,  370,    0,
  370,  370,    0,  370,  370,  370,  370,  370,  370,  370,
  370,  370,  370,  370,  370,  370,    0,  370,  370,  373,
  373,    0,    0,  370,  370,  370,  370,  370,    0,  370,
  370,  370,  370,  370,  370,  370,    0,  370,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  370,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  370,  370,  370,  370,    0,    0,    0,  370,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  373,  373,  373,    0,  373,  373,    0,
    0,    0,  373,    0,  373,    0,    0,  373,  373,  373,
  373,  373,  373,  373,  373,  373,  373,    0,  373,  373,
    0,  373,  373,  373,  373,  373,  373,  373,  373,  373,
  373,  373,  373,  373,    0,  373,  373,  393,  393,    0,
    0,  373,  373,  373,  373,  373,    0,  373,  373,  373,
  373,  373,  373,  373,    0,  373,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  373,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  373,  373,
  373,  373,    0,    0,    0,  373,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  393,  393,  393,    0,  393,  393,    0,    0,    0,
  393,    0,  393,    0,    0,  393,  393,  393,  393,  393,
  393,  393,  393,  393,  393,    0,  393,  393,    0,  393,
  393,  393,  393,  393,  393,  393,  393,  393,  393,  393,
  393,  393,    0,  393,  393,  391,  391,    0,    0,  393,
  393,  393,  393,  393,    0,  393,  393,  393,  393,  393,
  393,  393,    0,  393,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  393,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  393,  393,  393,  393,
    0,    0,    0,  393,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  391,
  391,  391,    0,  391,  391,    0,    0,    0,  391,    0,
  391,    0,    0,  391,  391,  391,  391,  391,  391,  391,
  391,  391,  391,    0,  391,  391,    0,  391,  391,  391,
  391,  391,  391,  391,  391,  391,  391,  391,  391,  391,
    0,  391,  391,  364,  364,    0,    0,  391,  391,  391,
  391,  391,    0,  391,  391,  391,  391,  391,  391,  391,
    0,  391,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  391,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  391,  391,  391,  391,    0,    0,
    0,  391,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  364,  364,  364,
    0,  364,  364,    0,    0,    0,  364,    0,  364,    0,
    0,  364,  364,  364,  364,  364,  364,  364,  364,  364,
  364,    0,  364,  364,    0,  364,  364,  364,  364,  364,
  364,  364,  364,  364,  364,  364,  364,  364,    0,  364,
  364,  369,  369,    0,    0,  364,  364,  364,  364,  364,
    0,  364,  364,  364,  364,  364,  364,  364,    0,  364,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  364,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  364,  364,  364,  364,    0,    0,    0,  364,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  369,  369,  369,    0,  369,
  369,    0,    0,    0,  369,    0,  369,    0,    0,  369,
  369,  369,  369,  369,  369,  369,  369,  369,  369,    0,
  369,  369,    0,  369,  369,  369,  369,  369,  369,  369,
  369,  369,  369,  369,  369,  369,    0,  369,  369,  371,
  371,    0,    0,  369,  369,  369,  369,  369,    0,  369,
  369,  369,  369,  369,  369,  369,    0,  369,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  369,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  369,  369,  369,  369,    0,    0,    0,  369,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  371,  371,  371,    0,  371,  371,    0,
    0,    0,  371,    0,  371,    0,    0,  371,  371,  371,
  371,  371,  371,  371,  371,  371,  371,    0,  371,  371,
    0,  371,  371,  371,  371,  371,  371,  371,  371,  371,
  371,  371,  371,  371,    0,  371,  371,  374,  374,    0,
    0,  371,  371,  371,  371,  371,    0,  371,  371,  371,
  371,  371,  371,  371,    0,  371,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  371,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  371,  371,
  371,  371,    0,    0,    0,  371,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  374,  374,  374,    0,  374,  374,    0,    0,    0,
  374,    0,  374,    0,    0,  374,  374,  374,  374,  374,
  374,  374,  374,  374,  374,    0,  374,  374,    0,  374,
  374,  374,  374,  374,  374,  374,  374,  374,  374,  374,
  374,  374,    0,  374,  374,  375,  375,    0,    0,  374,
  374,  374,  374,  374,    0,  374,  374,  374,  374,  374,
  374,  374,    0,  374,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  374,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  374,  374,  374,  374,
    0,    0,    0,  374,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  375,
  375,  375,    0,  375,  375,    0,    0,    0,  375,    0,
  375,    0,    0,  375,  375,  375,  375,  375,  375,  375,
  375,  375,  375,    0,  375,  375,    0,  375,  375,  375,
  375,  375,  375,  375,  375,  375,  375,  375,  375,  375,
    0,  375,  375,  392,  392,    0,    0,  375,  375,  375,
  375,  375,    0,  375,  375,  375,  375,  375,  375,  375,
    0,  375,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  375,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  375,  375,  375,  375,    0,    0,
    0,  375,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  392,  392,  392,
    0,  392,  392,    0,    0,    0,  392,    0,  392,    0,
    0,  392,  392,  392,  392,  392,  392,  392,  392,  392,
  392,    0,  392,  392,    0,  392,  392,  392,  392,  392,
  392,  392,  392,  392,  392,  392,  392,  392,    0,  392,
  392,  376,  376,    0,    0,  392,  392,  392,  392,  392,
    0,  392,  392,  392,  392,  392,  392,  392,    0,  392,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  392,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  392,  392,  392,  392,    0,    0,    0,  392,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  376,  376,  376,    0,  376,
  376,    0,    0,    0,  376,    0,  376,    0,    0,  376,
  376,  376,  376,  376,  376,  376,  376,  376,  376,    0,
  376,  376,    0,  376,  376,  376,  376,  376,  376,  376,
  376,  376,  376,  376,  376,  376,  350,  376,  376,    0,
    0,    0,    0,  376,  376,  376,  376,  376,    0,  376,
  376,  376,  376,  376,  376,  376,    0,  376,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  376,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  376,  376,  376,  376,    0,    0,    0,  376,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  352,  353,  354,    0,  355,  356,    0,    0,    0,  357,
    0,  358,    0,    0,  359,  360,  361,  362,  363,  364,
  365,  366,  367,  368,    0,  369,  370,    0,  371,  372,
  373,  374,  375,  376,  377,  378,  379,  380,  381,  382,
  383,  289,  384,  385,    0,    0,    0,    0,  386,  387,
  388,  389,  390,    0,  391,  392,  393,  394,  395,  396,
  397,    0,  398,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  399,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  400,  401,  402,  403,    0,
    0,    0,  404,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  289,  289,  289,    0,  289,
  289,    0,    0,    0,  289,    0,  289,    0,    0,  289,
  289,  289,  289,  289,  289,  289,  289,  289,  289,    0,
  289,  289,    0,  289,  289,  289,  289,  289,  289,  289,
  289,  289,  289,  289,  289,  289,  290,  289,  289,    0,
    0,    0,    0,  289,  289,  289,  289,  289,    0,  289,
  289,  289,  289,  289,  289,  289,    0,  289,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  289,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  289,  289,  289,  289,    0,    0,    0,  289,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  290,  290,  290,    0,  290,  290,    0,    0,    0,  290,
    0,  290,    0,    0,  290,  290,  290,  290,  290,  290,
  290,  290,  290,  290,    0,  290,  290,    0,  290,  290,
  290,  290,  290,  290,  290,  290,  290,  290,  290,  290,
  290,  291,  290,  290,    0,    0,    0,    0,  290,  290,
  290,  290,  290,    0,  290,  290,  290,  290,  290,  290,
  290,    0,  290,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  290,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  290,  290,  290,  290,    0,
    0,    0,  290,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  291,  291,  291,    0,  291,
  291,    0,    0,    0,  291,    0,  291,    0,    0,  291,
  291,  291,  291,  291,  291,  291,  291,  291,  291,    0,
  291,  291,    0,  291,  291,  291,  291,  291,  291,  291,
  291,  291,  291,  291,  291,  291,  292,  291,  291,    0,
    0,    0,    0,  291,  291,  291,  291,  291,    0,  291,
  291,  291,  291,  291,  291,  291,    0,  291,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  291,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  291,  291,  291,  291,    0,    0,    0,  291,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  292,  292,  292,    0,  292,  292,    0,    0,    0,  292,
    0,  292,    0,    0,  292,  292,  292,  292,  292,  292,
  292,  292,  292,  292,    0,  292,  292,    0,  292,  292,
  292,  292,  292,  292,  292,  292,  292,  292,  292,  292,
  292,    0,  292,  292,    0,    0,    0,    0,  292,  292,
  292,  292,  292,    0,  292,  292,  292,  292,  292,  292,
  292,    0,  292,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  292,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  355,    0,  292,  292,  292,  292,    0,
  358,    0,  292,  359,  360,  361,  362,  363,  364,  365,
  366,  367,  368,    0,  369,  370,    0,  371,  372,  373,
  374,  375,  376,  377,  378,  379,  380,  381,  382,  383,
    0,  384,  385,    0,    0,    0,    0,  386,  387,  388,
  389,  390,    0,  391,  392,  393,  394,  395,  396,  397,
    0,  398,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  399,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  400,  401,  402,  403,    0,    0,
    0,  404,
  };
  protected static readonly short [] yyCheck = {             6,
   71,   41,    6,   67,  596,   44,   41,   20,   79,   16,
   40,   60,  322,  323,  324,  194,  611,   30,  134,   44,
   62,   40,   33,   40,   91,   41,    6,   41,   93,   41,
   40,  341,  653,   41,   60,  195,   16,   40,   44,   41,
   40,   33,   91,   41,  405,   41,  123,   44,  137,   44,
  280,   44,   61,   40,   44,  283,   63,  121,  307,  316,
   44,  123,  123,   70,   71,   44,  274,   44,  123,   44,
   44,   44,   79,  280,  123,   82,  123,  193,   82,   44,
  280,   94,  291,   96,   44,   98,   99,  296,   95,  102,
   70,   71,  123,   44,   44,   44,   44,  123,   61,   79,
  123,  123,   82,  110,  111,  222,  223,   41,  197,  276,
   44,  200,  119,   44,  281,  834,  887,  124,  189,  125,
  127,  134,  129,  283,   44,  129,  133,  384,  125,  136,
   41,   61,  136,   44,  294,  263,  143,  908,  280,  734,
  147,  451,   62,  150,  151,  273,  153,  127,  276,  129,
   44,  381,   40,  774,   42,  776,  136,  280,  779,  348,
   44,   44,  559,  352,  171,  172,   44,  174,   41,  270,
  271,   44,  314,  430,  381,  266,  307,  293,  292,  293,
  257,  381,  953,  325,  955,   61,  199,  436,  437,  417,
  124,  314,  325,  310,  125,  286,  203,  204,  917,   93,
  272,  972,  325,  292,  335,  336,  475,  260,  477,  570,
  419,  325,  274,  274,  381,  264,   61,  614,  615,  274,
  296,  297,  229,  230,  273,   71,  233,  274,  235,   40,
  409,   42,  411,  282,  283,  284,  285,  286, 1009,  381,
  289,  125,  125,  274,    0,  840,  474,  125,   94,   60,
   96,  274,  274,   61,  875,  272,  384,  264,  381,  384,
  420,  310,  288,  274,   40,  360,  277,  362,  274,  272,
  384,   40,  272,   40,  369,  282,  280,  274,  291,  257,
   91,   40,  274,  296,  355,  272,  596,   41,  598,   41,
   44,  355,   44,  603,  123,   40,  303,   42,  281,   44,
  697,  698,  257,  310,  701,  436,  437,   40,  274,   40,
 1081, 1082,  123,  274,   60,  322,  323,  384,   41,  384,
  915,   44,   41,  292,  293,   44,   41,  296,  297,   44,
  349,  926,  927,   61,  341,  645,  292,  293,  402,  349,
  296,  297,  384,  740,  384,   91,  260,   41,  355,  384,
   44,  358,  382,  360,  323,  362,  260,  364,  365,  384,
  367,  368,  369,  370,  371,  372,  373,  374,  384,   40,
  384,  317,  384,  380,  381,  355,  384,  123,  385,   20,
  125,   40,  384,   40,  979,   40,  384,  566,  384,  384,
  982,  384,  441,  400,  384,  402,  257,  404,  402,  994,
  384,   60,  997,  442,  276,  384,  419,  384,  587,  384,
  384,  384,  809,  810,  485,  384,  813,  814,  267,  384,
  427,   40,  402,   42,  384,   44,   41,  606,  384,   44,
  267,   42,   91,  384,  384,  384,  384, 1032, 1033,   40,
   41,   42,   41,   44,  274,   44,  257,  258,  259,  123,
  261,  262,  263,   94,  265,   96,  257,   98,   99, 1054,
   41,  272,  273,   44,  123,  257,  258,  259,  279,  261,
  262,  263,   41,  265,    0,   44,  287,  484,  485,   41,
  484,   41,   44,  490,   44,  317,  493,  279,  495,  496,
  123,  498,  499,  134,  501,  287,  503,   40,  562,   41,
  507,  508,   44,  510,  484,  485,  125,  904,   41,  123,
  257,   44,  268,  269,  521,  522,  272,  273,  264,  275,
   41,  528,  529,  530,  531,  381,  361,  273,  363,   41,
  709,  366,   44,   41,  290,  291,  282,  283,  284,  285,
  286,  325,   40,  289,  723,  724,   40,    0,  555,   40,
   40,  558,   44,  560,  561,  562,   58,  561,  562,   58,
  870,  871,   58,  163,  399,   41,  401,  276,  257,  380,
  326,  578,   40,   62,  581,  582,  583,  584,   61,  260,
  560,  561,  562,  381,  395,  396,  397,  398,  257,  596,
   44,  598,  384,  381,  381,   44,  603,  123,  257,  258,
  259,   44,  261,  262,  263,   44,  265,  260,  260,  616,
  920,  257,  616,  272,  273,   61,  427,   58,   58,  421,
  279,  221,  222,  223,  334,  424,  268,  269,  287,  314,
  272,  273,  703,  275,  257,  381,  616,   40,  381,   42,
  819,  390,  649,  243,  651,   44,  653,   40,  290,  291,
  657,  390,  831,  660,  661,  662,  663,  664,  665,  666,
  667,  668,  669,  670,  671,  390,  490,  257,  390,  257,
  123,  495,  982,  381,  498,   40,   41,   42,  381,   44,
  381,   44,  689,  507,  326,   44,   40,  287,  124,  805,
  806,  273,  699,  700,   44,  702,  703,   44,   44,   44,
   44,   44,   44,  390,  304,  305,  306,  307,  390,  260,
  310,  390,  390,  390,  390,  390,  257,  390,  390,  699,
  700,  380,  702,  703,  390,  390,  390,  257,   62,  736,
  257,  331,  257,  257,  894,  895,  395,  396,  397,  398,
  260,  381,  268,  269,  257,  381,  272,  273,  274,  275,
   44,  266,  352,   40,  354,   44,  356,  260,   44,   44,
   44,   44,   44,   44,  290,  291,  837,  774,  427,  776,
  296,  297,  779,   44,   44,  375,  376,  377,  378,  379,
  896,  897,  382,   44,  900,  901,  386,  387,  388,  389,
  390,  391,  392,  393,  394,  395,  396,  397,   44,  325,
  326,   44,  962,  963,   44,   44,  966,  967,  815,   44,
  428,  815,  257,   44,   44,  268,  269,   91,  390,  272,
  273,  274,  275,  257,  257,   44,   44,  834,  835,   44,
   44,   44,   44,   44,  434,  815,   44,  290,  291,   44,
   44,  146,   44,  296,  297,   44,   44,   44,  964,  965,
   44,  257,  968,  969,   44, 1015, 1016, 1017, 1018,   44,
   60,  390,   44,   44,  317,    0,   44,   44,  875,   44,
  273,  274,  325,  326,   44,  882,  883,   60,   44,    0,
  260,   91,   44,   44, 1044,   44,  381,   44,  381,    0,
  390,   91,   44,  381,   44,  381,   44,  202,   44,   44,
   44,  381,   44, 1019,  257,   40,   44,   42,   91,   40,
  917,  329,  390,   93,  381,  381,  257,  924,  922,   40,
  257,   42,  381,  123,  524,  257,  231,  232,   44,  234,
   44,   44,  260,  257,  337,  338,  339,   40,  341,  342,
  123,   40,   40,  346,  249,  348,  329,  382,  351,  352,
  353,  354,  355,  356,  357,  358,  359,  360,  329,  362,
  363,  257,  365,  366,  367,  368,  369,  370,  371,  372,
  373,  374,  375,  376,  377,  982,  379,  380,   44,  381,
  580,  257,  385,  386,  387,  388,  389,  381,  391,  392,
  393,  394,  395,  396,  397,  300,  399,  257,    0,  334,
  329,  334,  257,   44,   44,   40,  390,  410,    0,  390,
  257,  390,  381,  381,  319,  257,  321,   44,   44,  422,
  423,  424,  425,  257,  258,  259,  429,  261,  262,  263,
  257,  265,    0,   10,  272,  272,  272,    0,   40,  317,
   42, 1048,  642,  643,  644,  279,  646,  647,   40,  272,
   42,  272, 1059,  287, 1061, 1062, 1063,  103,  242,  659,
   20,  326,  598,  119,  264,  323,  414,  243,  277,  264,
   67,  976,  408,  273,  870,  282,  871,   40,  408,   42,
  476,  264,  282,  283,  284,  285,  286,  603,  924,  289,
  273,  802,  917,  701,  294,   -1,    0,   -1,  476,  282,
  283,  284,  285,  286,   -1,  705,  289,  307,  308,  309,
   -1,  311,  312,   -1,  714,  315,   -1,   -1,  318,  319,
  320,  321,  322,   -1,  324,   -1,   -1,   -1,   -1,   -1,
  435,   -1,   -1,  268,  269,    0,   -1,  272,  273,   -1,
  275,   -1,   -1,   -1,   -1,   -1,   -1,  268,  269,  125,
   -1,  272,  273,   -1,  275,  290,  291,  268,  269,   -1,
   -1,  272,  273,   -1,  275,   -1,  349,  350,   -1,  290,
  291,   -1,   -1,   40,   -1,  480,   -1,   -1,   -1,  290,
  291,  381,   -1,  783,  489,  785,   -1,   -1,   -1,  494,
   -1,  326,  497,   60,   -1,  500,   -1,  502,   -1,  504,
  505,  506,   -1,   -1,  509,  326,  511,  512,  513,  514,
   -1,   -1,   -1,   -1,   -1,  326,   -1,   -1,   -1,   -1,
  820,  421,   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  433,  434,  435,  436,  437,  438,  439,
  440,  292,  293,   -1,   -1,  296,  297,  298,  299,  300,
  301,  302,  303,  304,  305,  306,  123,   -1,   -1,   -1,
   -1,   -1,  313,   60,   -1,   -1,  268,  269,  868,  869,
  272,  273,  323,  275,   -1,   -1,  268,  269,   -1,   -1,
  272,  273,   -1,  275,  260,  885,   -1,   -1,  290,  291,
   -1,   -1,  892,   -1,   91,   -1,    0,   -1,  290,  291,
  276,   -1,   -1,   -1,   -1,  268,  269,   -1,   -1,  909,
   -1,   -1,  275,  618,   -1,  620,   -1,  622,  623,   -1,
  625,  626,   -1,  628,  326,  630,  123,  290,  291,  634,
  635,   -1,  637,  384,  326,   -1,   -1,   -1,   -1,   -1,
    0,   -1,   -1,   -1,  320,  321,  322,   -1,   -1,  654,
  655,  327,  328,   -1,  330,  331,  332,  333,   -1,   -1,
   -1,   -1,   -1,  326,  268,  269,   -1,  672,  272,  273,
   -1,  275,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   40,   41,   42,   -1,   44,  690,  290,  291,  693,   -1,
  257,  258,  259,   -1,  261,  262,  263,   -1,  265,   -1,
   60,   -1,   62,  268,  269,  272,  273,  272,  273,   -1,
  275,   -1,  279, 1013,   -1,   -1,   -1,   -1,   -1,   -1,
  287,   -1,  326,   -1,   -1,  290,  291,   -1, 1028, 1029,
 1030,   91,   -1,   93,   -1,   -1,  741,   -1,   -1,  744,
   -1,   -1,  747,   -1,   -1,  750,   -1,  752,   -1,  754,
  755,  756,  317,   -1,  759,   -1,  761,  762,  763,  764,
   -1,  326,   -1,  123,   -1,  125,   -1,  264,   -1, 1069,
   -1,   -1,  777,   -1,   -1, 1075,  273,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,  800,   -1,  294,   -1,   -1,
   -1,   -1,  299,   -1,  301,  302,  303,  304,  305,  306,
  307,  308,  309,  380,  311,  312,  313,   -1,  315,   -1,
   -1,  318,  319,  320,  321,  322,   -1,  324,  395,  396,
  397,  398,  837,   -1,   -1,   -1,    0,   -1,  335,  336,
  845,  846,   -1,  848,  849,   -1,  851,  852,   -1,  854,
  125,  856,   -1,   -1,   -1,  860,  861,   -1,  863,   -1,
   -1,   -1,   -1,   -1,  268,  269,   -1,   -1,  272,  273,
   -1,  275,   -1,   -1,   -1,   41,   -1,   -1,   44,   -1,
   -1,   -1,   -1,  888,  381,  890,  290,  291,   -1,   -1,
   -1,   -1,   -1,   -1,   60,   -1,   -1,  257,  258,  259,
    0,  261,  262,  263,   -1,  265,   -1,   -1,  268,  269,
    0,   -1,  272,  273,  274,  275,  276,  922,   -1,  279,
    0,   -1,  326,   -1,   -1,   91,   -1,  287,   -1,   -1,
  290,  291,   -1,   -1,  294,   -1,  433,  434,  435,  436,
  437,  438,  439,  440,   -1,   -1,  951,  952,  308,  309,
   -1,  311,  312,  958,   -1,  315,  316,  123,  318,  319,
  320,  321,  322,   -1,  324,    0,  326,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,    0,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,  260,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,  276,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  381,   -1,  123,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,  398,  399,
   -1,    0,   -1,   -1,   -1,  320,  321,  322,   -1,   -1,
  410,   -1,  327,  328,   -1,  330,  331,  332,  333,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,  427,   -1,  429,
  430,   -1,   -1,  433,  434,  435,   -1,   -1,  438,  439,
  440,   40,   41,   42,   -1,   44,   -1,   -1,   -1,   -1,
   -1,  257,  258,  259,   -1,  261,  262,  263,  264,  265,
   -1,   60,   -1,   62,  268,  269,  272,  273,  272,  273,
   -1,  275,   -1,  279,   -1,   -1,  282,  283,  284,  285,
  286,  287,   -1,  289,   -1,   -1,  290,  291,  294,   -1,
   -1,   -1,   91,   -1,   93,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  308,  309,   -1,  311,  312,   -1,   -1,  315,
   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,   -1,
   -1,   -1,  326,   -1,  123,   -1,  125,   -1,  268,  269,
   -1,   -1,  272,  273,  274,  275,   -1,   -1,  268,  269,
   -1,   -1,  272,  273,   -1,  275,   -1,    0,  268,  269,
  290,  291,  272,  273,   -1,  275,  296,  297,   -1,   -1,
  290,  291,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  290,  291,   -1,   -1,  380,  381,   -1,  317,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  325,  326,   -1,   -1,  395,
  396,  397,  398,  268,  269,   -1,  326,  272,  273,   -1,
  275,   -1,   -1,  268,  269,   -1,  326,  272,  273,   -1,
  275,   -1,   -1,   -1,   -1,  290,  291,   -1,    0,   -1,
   -1,   -1,   -1,   -1,   -1,  290,  291,  433,  434,  435,
   -1,   -1,  438,  439,  440,   -1,   41,   -1,   -1,   44,
   -1,   -1,   -1,   94,   -1,   96,   97,   98,   99,   -1,
   -1,  326,   -1,   -1,   -1,   60,   -1,   -1,  257,  258,
  259,  326,  261,  262,  263,   -1,  265,   -1,    0,  268,
  269,   -1,   -1,  272,  273,  274,  275,  276,   -1,   -1,
  279,   -1,   -1,   -1,   -1,   -1,   91,   -1,  287,   -1,
   -1,  290,  291,  144,  145,  294,    0,  148,  149,   -1,
   -1,  152,   -1,  154,   -1,    0,   -1,   -1,   -1,  308,
  309,   -1,  311,  312,   -1,   -1,  315,  316,  123,  318,
  319,  320,  321,  322,   -1,  324,   -1,  326,   -1,    0,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,
  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,
   -1,   -1,  351,  352,  353,  354,  355,  356,  357,  358,
  359,  360,   -1,  362,  363,   -1,  365,  366,  367,  368,
  369,  370,  371,  372,  373,  374,  375,  376,  377,    0,
  379,  380,   -1,   -1,   -1,  384,  385,  386,  387,  388,
  389,   -1,  391,  392,  393,  394,  395,  396,  397,  398,
  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  125,  410,   -1,   -1,   -1,  268,  269,   -1,   -1,  272,
  273,   -1,  275,  422,  423,  424,  425,   -1,  427,   -1,
  429,  430,   -1,   -1,  433,  434,  435,  290,  291,  438,
  439,  440,   40,   41,   -1,   -1,   44,   -1,   -1,   -1,
   -1,   -1,  257,  258,  259,   -1,  261,  262,  263,  264,
  265,   60,   -1,   -1,   62,   -1,   -1,  272,  273,   -1,
   -1,   -1,   -1,  326,  279,   -1,   -1,  282,  283,  284,
  285,  286,  287,   44,  289,   -1,  268,  269,   -1,  294,
  272,  273,   91,  275,   -1,   93,   -1,   -1,   -1,   60,
   -1,   -1,   -1,  308,  309,   -1,  311,  312,  290,  291,
  315,   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,
   -1,   -1,   -1,   -1,  123,  123,  125,  125,  125,   -1,
   91,   -1,   -1,   -1,   -1,   -1,  268,  269,   -1,   -1,
  272,  273,   -1,  275,  326,  260,   -1,   -1,   -1,   -1,
   40,   -1,   42,   -1,   -1,   -1,   -1,   -1,  290,  291,
   -1,  276,  123,   -1,  268,  269,   -1,   -1,  272,  273,
   -1,  275,   -1,  268,  269,  380,  381,  272,  273,   -1,
  275,   -1,   -1,   -1,   -1,   -1,  290,  291,   -1,   -1,
  395,  396,  397,  398,  326,  290,  291,  268,  269,   -1,
   -1,  272,  273,   -1,  275,  320,  321,  322,   -1,   -1,
   -1,   -1,  327,  328,   -1,  330,  331,  332,  333,  290,
  291,   -1,  326,   -1,   -1,   -1,   -1,   -1,  433,  434,
  435,  326,   -1,  438,  439,  440,  400,  401,  402,  403,
  404,  405,  406,  407,  408,  409,   -1,  268,  269,   -1,
   -1,  272,  273,    0,  275,  326,   -1,   -1,   40,  257,
   42,   -1,   -1,  260,   -1,  264,   -1,   -1,   -1,  290,
  291,   -1,   -1,   -1,  273,  273,  274,   -1,  276,  276,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,   -1,   -1,   -1,   -1,   -1,  257,  258,  259,   -1,
  261,  262,  263,   -1,  265,  326,   -1,   -1,   -1,   -1,
   -1,  272,  273,   -1,   -1,   -1,   -1,   -1,  279,   -1,
   -1,   -1,   -1,  320,  321,  322,  287,   -1,   -1,   -1,
  327,  328,   -1,  330,  331,  332,  333,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,  390,  391,  392,  393,  394,  395,  396,  397,
   40,  399,   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,  380,
   -1,   -1,   -1,  384,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,  395,  396,  397,  398,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   40,  399,
   42,  261,  262,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,  268,  269,   -1,   -1,  272,  273,   -1,  275,   -1,
  261,  262,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,  290,  291,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   60,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,  326,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,   91,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   40,  399,   42,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,  123,   -1,  273,  274,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,  402,  403,  404,  405,   -1,   -1,   -1,   -1,
   -1,  411,  412,  413,  414,  415,  416,  417,  418,  419,
  420,  402,  403,  404,  405,   -1,   -1,   -1,   -1,   -1,
  411,  412,  413,  414,  415,  416,  417,  418,  419,  420,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   40,  399,
   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  273,  422,  423,  424,  425,   -1,   -1,   -1,  429,
  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   40,  399,   42,   -1,
   -1,   -1,  364,   -1,   -1,   -1,   -1,   -1,  410,  257,
  258,  259,   -1,  261,  262,  263,   -1,  265,   -1,   -1,
  422,  423,  424,  425,  272,  273,   -1,  429,  276,   -1,
   -1,  279,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  287,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   60,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   91,   -1,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   40,  399,   42,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,  123,
   -1,  273,  274,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,  380,   -1,   -1,  429,  384,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  395,  396,  397,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   40,  399,   42,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,
  422,  423,  424,  425,   -1,   -1,   -1,  429,  282,  283,
  284,  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   40,  399,   42,   -1,   -1,   -1,
  364,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   60,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,   91,   -1,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   40,  399,   42,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,  123,   -1,  273,
  274,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,  264,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,  422,  423,
  424,  425,   -1,   -1,   -1,  429,  282,  283,  284,  285,
  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   60,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,  695,  696,   -1,   -1,  364,   -1,
   -1,   -1,   -1,   91,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  123,   -1,  738,  739,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,   -1,   -1,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,  807,  808,   -1,   -1,
  811,  812,  410,   -1,   -1,   60,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  843,   -1,   -1,   -1,   91,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   60,   -1,   -1,   -1,  257,
  258,  259,   -1,  261,  262,  263,   -1,  265,  123,   60,
   -1,   -1,   -1,   -1,  272,  273,   -1,   -1,   -1,   -1,
   -1,  279,   -1,   -1,   -1,   -1,   91,  898,  899,  287,
   -1,  902,  903,   -1,   -1,   -1,  294,   -1,   -1,   -1,
   91,   -1,   -1,   -1,   -1,   60,   -1,   -1,   -1,   -1,
  308,  309,   -1,  311,  312,   -1,   -1,  315,  123,   -1,
  318,  319,  320,  321,  322,   -1,  324,   -1,   -1,   -1,
   -1,   -1,  123,   -1,   -1,   -1,   91,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   60,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  970,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  123,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,
   -1,   -1,  380,  381,   -1,   -1,  384,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  395,  396,  397,
  398,   -1,  257,  258,  259,   -1,  261,  262,  263,   -1,
  265,  123,   -1,   -1,   -1,   -1,   -1,  272,  273,   -1,
   -1,   -1,   -1,   -1,  279,   -1,   -1,   -1,   -1,   44,
   -1,   -1,  287,   -1,   -1,  433,  434,  435,   -1,  294,
  438,  439,  440,   -1,   -1,   60,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  308,  309,   -1,  311,  312,   -1,  264,
  315,   -1,   -1,  318,  319,  320,  321,  322,  273,  324,
   -1,   -1,   -1,  264,   -1,   -1,   91,  282,  283,  284,
  285,  286,  273,   -1,  289,   -1,   -1,   -1,   -1,  294,
   -1,  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,
   -1,   -1,  307,  308,  309,   -1,  311,  312,  123,  264,
  315,   -1,   -1,  318,  319,  320,  321,  322,  273,  324,
   -1,   -1,   -1,   -1,   -1,  380,  381,  282,  283,  284,
  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,  294,
  395,  396,  397,  398,   -1,   -1,   -1,   -1,   -1,   -1,
   60,   -1,  264,  308,  309,   -1,  311,  312,   -1,   -1,
  315,  273,   -1,  318,  319,  320,  321,  322,  323,  324,
  282,  283,  284,  285,  286,   -1,  381,  289,  433,  434,
  435,   91,  294,  438,  439,  440,   -1,   -1,   -1,   -1,
   -1,   -1,  383,   60,   -1,   -1,  308,  309,   -1,  311,
  312,   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,
  322,  323,  324,  123,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   91,   -1,  381,   -1,  433,  434,
  435,  436,  437,  438,  439,  440,   60,   -1,   -1,   -1,
   -1,   -1,  257,  258,  259,   -1,  261,  262,  263,   -1,
  265,   -1,   -1,   -1,   -1,   -1,  123,  272,  273,   -1,
   -1,   -1,   -1,   -1,  279,   -1,   -1,   91,   -1,  381,
   -1,   -1,  287,   -1,   -1,   -1,   -1,   -1,  433,  434,
  435,   -1,   -1,  438,  439,  440,   60,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  123,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,   -1,   -1,
   -1,  433,  434,  435,   -1,   -1,  438,  439,  440,   60,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  123,
   -1,   -1,   -1,   -1,  264,   -1,   -1,   -1,   -1,   -1,
   91,   -1,   -1,  273,   -1,  380,   -1,   -1,   -1,  384,
   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,  289,
  395,  396,  397,  398,  294,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  123,   -1,   -1,   -1,   -1,  264,  308,  309,
   60,  311,  312,   -1,   -1,  315,  273,   -1,  318,  319,
  320,  321,  322,  323,  324,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,  294,   -1,   -1,
   -1,   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  264,  308,  309,   60,  311,  312,   -1,   -1,  315,  273,
   60,  318,  319,  320,  321,  322,   -1,  324,  282,  283,
  284,  285,  286,  123,   -1,  289,   -1,   -1,   -1,   -1,
  294,  381,   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,
   -1,   91,   -1,   -1,  308,  309,   -1,  311,  312,   -1,
  264,  315,   -1,   -1,  318,  319,  320,  321,  322,  273,
  324,   -1,   -1,   -1,   -1,   -1,  123,   -1,  282,  283,
  284,  285,  286,  123,  381,  289,   -1,   -1,   -1,   -1,
  294,   -1,   -1,  433,  434,  435,   -1,   -1,  438,  439,
  440,   -1,   -1,  264,  308,  309,   -1,  311,  312,   -1,
   -1,  315,  273,   -1,  318,  319,  320,  321,  322,   -1,
  324,  282,  283,  284,  285,  286,   -1,  381,  289,   -1,
   -1,   -1,   -1,  294,   -1,   -1,  433,  434,  435,   -1,
   -1,  438,  439,  440,  125,   -1,   -1,  308,  309,   -1,
  311,  312,   -1,   -1,  315,   -1,   -1,  318,  319,  320,
  321,  322,   -1,  324,   -1,   -1,   -1,  257,  258,  259,
   -1,  261,  262,  263,   -1,  265,   -1,  381,   -1,  433,
  434,  435,  272,  273,  438,  439,  440,   -1,   -1,  279,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  287,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  257,  258,  259,   -1,  261,  262,  263,   -1,  265,   -1,
  381,   -1,   -1,   -1,  264,  272,  273,   -1,   -1,  433,
  434,  435,  279,  273,  438,  439,  440,   -1,   -1,   -1,
  287,   -1,  282,  283,  284,  285,  286,   -1,   -1,  289,
   60,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  125,
   -1,   -1,  433,  434,  435,   -1,  257,  438,  439,  440,
   -1,   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  380,   -1,  273,   -1,  384,  276,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  395,  396,  397,  398,  349,
   -1,   -1,   -1,  123,   -1,  125,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  380,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  382,   -1,   -1,   -1,   -1,   -1,  395,  396,
  397,  398,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,
  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,
  351,  352,  353,  354,  355,  356,  357,  358,  359,  360,
   -1,  362,  363,   -1,  365,  366,  367,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,   -1,  379,  380,
   -1,  257,  125,   -1,  385,  386,  387,  388,  389,   -1,
  391,  392,  393,  394,  395,  396,  397,  273,  399,   -1,
  276,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,
   -1,   -1,   -1,   -1,  264,  265,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  273,  274,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,  289,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   60,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,   91,   -1,  125,   -1,  385,
  386,  387,  388,  389,  257,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
  273,   -1,   -1,  276,  410,   -1,   -1,  123,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,
   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,
  353,  354,  355,  356,  357,  358,  359,  360,   -1,  362,
  363,   -1,  365,  366,  367,  368,  369,  370,  371,  372,
  373,  374,  375,  376,  377,   -1,  379,  380,   -1,  257,
  125,   -1,  385,  386,  387,  388,  389,   -1,  391,  392,
  393,  394,  395,  396,  397,  273,  399,   -1,  276,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  264,  422,
  423,  424,  425,   -1,   -1,   -1,  429,  273,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,
  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  349,   -1,  125,   -1,  385,  386,  387,
  388,  389,  257,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,
   -1,  276,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,
   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,
  355,  356,  357,  358,  359,  360,   -1,  362,  363,   41,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  380,   -1,  257,   60,   -1,
  385,  386,  387,  388,  389,   41,  391,  392,  393,  394,
  395,  396,  397,  273,  399,   -1,  276,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   60,  410,   -1,   -1,   -1,   91,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,
  425,   60,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   91,   -1,   -1,   60,   -1,
   -1,  123,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   91,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,  123,  348,   91,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,  123,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  123,   41,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   60,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   41,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   91,   -1,   -1,   60,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   41,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  264,   -1,  123,   60,   91,   -1,   -1,   -1,
   -1,  273,   -1,   -1,   -1,   -1,  278,   -1,   -1,   -1,
  282,  283,  284,  285,  286,   -1,   -1,  289,  264,   60,
   -1,   -1,   -1,  295,   -1,   -1,   91,  273,  123,   -1,
   -1,   -1,   -1,   -1,   60,  264,  282,  283,  284,  285,
  286,   -1,   -1,  289,  273,   -1,   -1,   -1,   -1,  295,
   91,   -1,  264,  282,  283,  284,  285,  286,  123,   -1,
  289,  273,   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,
  282,  283,  284,  285,  286,   60,   -1,  289,   -1,   -1,
   -1,  310,  123,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  123,  310,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   91,   -1,   60,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  257,  258,
  259,   -1,  261,  262,  263,  264,  265,   -1,  123,   91,
   -1,   60,   -1,   -1,  273,   -1,   -1,  276,   -1,   60,
  279,   -1,   -1,  282,  283,  284,  285,  286,  287,   -1,
  289,   -1,  257,  258,  259,   -1,  261,  262,  263,  264,
  265,  123,   91,   -1,   -1,   -1,   60,   -1,  273,   -1,
   91,  276,   -1,   -1,  279,   -1,   -1,  282,  283,  284,
  285,  286,  287,   -1,  289,   -1,   -1,   -1,   -1,  264,
   -1,   -1,   60,   -1,  123,   -1,   -1,   91,  273,   -1,
   -1,   -1,  123,  278,  125,   -1,   -1,  282,  283,  284,
  285,  286,   -1,  264,  289,   -1,   -1,   -1,   -1,   -1,
   60,   -1,  273,   91,   -1,   -1,   -1,  278,  264,  123,
   -1,  282,  283,  284,  285,  286,   -1,  273,  289,   -1,
   -1,   -1,   -1,   60,  295,   -1,  282,  283,  284,  285,
  286,   91,   -1,  289,   -1,  123,   -1,   -1,   -1,  295,
   -1,   -1,  257,  258,  259,  260,  261,  262,  263,  264,
  265,   -1,   -1,   -1,   91,   -1,   93,   60,  273,  274,
   -1,  276,   -1,  123,  279,   60,   -1,  282,  283,  284,
  285,  286,  287,   -1,  289,  257,  258,  259,  260,  261,
  262,  263,  264,  265,   -1,   -1,  123,   -1,   91,   -1,
   -1,  273,  274,   -1,  276,   60,   91,  279,   -1,   -1,
  282,  283,  284,  285,  286,  287,   -1,  289,  257,  258,
  259,   -1,  261,  262,  263,  264,  265,   -1,   60,   -1,
  123,   -1,   -1,  264,  273,   -1,   91,  276,  123,   -1,
  279,   -1,  273,  282,  283,  284,  285,  286,  287,   -1,
  289,  282,  283,  284,  285,  286,   -1,   60,  289,   91,
  264,  265,   -1,   -1,   -1,   -1,   -1,   -1,  123,  273,
  274,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,
  284,  285,  286,   -1,   -1,  289,  264,  265,   91,   -1,
   -1,  123,   -1,   -1,   -1,  273,  274,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,
   -1,  289,   -1,   -1,  264,   -1,   -1,   -1,   -1,   -1,
  123,   -1,   -1,  273,  274,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  282,  283,  284,  285,  286,  264,   -1,  289,
   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  264,
  273,   -1,   -1,   -1,   -1,  278,   -1,   -1,  273,  282,
  283,  284,  285,  286,   -1,   -1,  289,  282,  283,  284,
  285,  286,   -1,   -1,  289,   -1,   -1,  257,   -1,  264,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,
   -1,   -1,   -1,  273,   -1,   -1,  276,  282,  283,  284,
  285,  286,  264,   -1,  289,   -1,   -1,   -1,   -1,   -1,
   -1,  273,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,   -1,
   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  273,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,
  283,  284,  285,  286,   -1,   -1,  289,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,   -1,   -1,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,  273,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,  294,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,  308,  309,   -1,  311,  312,  294,   -1,
  315,   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,
   -1,   -1,  308,  309,   -1,  311,  312,   -1,   -1,  315,
   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  381,   -1,   -1,  384,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  273,  274,   -1,   -1,  381,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  433,  434,
  435,   -1,   -1,  438,  439,  440,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  433,  434,  435,
   -1,   -1,  438,  439,  440,  337,  338,  339,   -1,  341,
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
  372,  373,  374,  375,  376,  377,  273,  379,  380,   -1,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,  273,  379,  380,   -1,   -1,   -1,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,
   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,  273,  379,  380,   -1,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,  273,  379,  380,   -1,   -1,   -1,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,
   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,  273,  379,  380,   -1,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,   -1,  379,  380,   -1,   -1,   -1,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  341,   -1,  422,  423,  424,  425,   -1,
  348,   -1,  429,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,   -1,   -1,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,
  };

#line 1523 "Iril/IR/IR.jay"

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
  public const int SWIFTCC = 436;
  public const int SWIFTTAILCC = 437;
  public const int SWIFTSELF = 438;
  public const int SWIFTERROR = 439;
  public const int SWIFTASYNC = 440;
  public const int ATOMIC = 441;
  public const int MONOTONIC = 442;
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
